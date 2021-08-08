using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MeshAnimation.Animation;
using MeshAnimation.Clustering;
using OpenTkRenderer.Rendering.Meshes;
using OpenTkRenderer.Structs;
using System;
using System.Collections.Generic;
using Supercluster.KDTree;
using MeshAnimation.DataStructures;

namespace MeshAnimation.Optimization
{
    class SSDROptimizer : IOptimizer
    {
        public int boneCount = 9;
        public int maxIterations = 1000;
        int maxInits = 10;

        SkinningAnimation outAnim;
        IAnimation inAnim;

        public float sigEpsilon;
        int initCount = 0;

        KDTree<double, int> tree;

        /// <summary>
        /// Optimize animation
        /// </summary>
        /// <returns></returns>
        public SkinningAnimation Optimize(IAnimation inAnim)
        {
            // TODO two different paths for TVM and DMA

            initCount = 0;
            outAnim = new SkinningAnimation((ObjLoader)inAnim.RestPose, boneCount, inAnim.Frames.Length);
            this.inAnim = inAnim;

            // initialize
            InitializationStep();

            // start loop
            int iteration = 0;
            while(true)
            {
                // update weights
                WeightUpdateStep();
                // update bone transformations
                BoneTransformUpdateStep();

                // check if converged
                if (CheckConvergence() || iteration > maxIterations)
                    break;

                iteration++;
            }

            CorrectRestPose();

            return outAnim;
        }

        /// <summary>
        /// Initialization step
        /// </summary>
        /// <param name="inAnim"> Input animated mesh sequence </param>
        /// <param name="outAnim"> Output skinning animation </param>
        private void InitializationStep()
        {
            // clustering
            KMeans km = Cluster(inAnim);
            // clustering into weight map
            PrepareOutAnimation(inAnim, km);
        }

        /// <summary>
        /// Create init clusters on vertices of animated mesh sequence
        /// </summary>
        /// <param name="inAnim"> Input animated mesh sequence </param>
        /// <returns> Kmeans implementation used for clustering</returns>
        private KMeans Cluster(IAnimation inAnim)
        {
            KMeans km = new KMeans();
            km.BoneCount = boneCount;

            ObjLoader[] objs = new ObjLoader[inAnim.Frames.Length];
            for (int f = 0; f < inAnim.Frames.Length; f++)
                objs[f] = (ObjLoader)inAnim.Frames[f];

            km.Cluster(objs);
            return km;
        }

        /// <summary>
        /// Put init clustering results stored in km into output animation outAnim
        /// </summary>
        /// <param name="inAnim"> Input animated mesh sequence </param>
        /// <param name="outAnim"> Output skinning animation </param>
        /// <param name="km"> Used kmeans implementation </param>
        private void PrepareOutAnimation(IAnimation inAnim, KMeans km)
        {
            // set rest pose
            outAnim.RestPose = (ObjLoader)inAnim.RestPose;

            // set weight map
            for (int i = 0; i < km.BoneClusters.Count; i++)
                for (int j = 0; j < km.BoneClusters[i].Length; j++)
                    outAnim.VertexBoneWeights[km.BoneClusters[i][j]].Add(i, 1);

            // set transformations
            for (int f = 0; f < inAnim.Frames.Length; f++)
            {
                for (int i = 0; i < boneCount; i++)
                {
                    outAnim.Frames[f].BoneRotation[i] = km.tMatrices[f][i];
                    outAnim.Frames[f].BoneTranslation[i] = km.tVectors[f][i];
                }
            }

        }

        /// <summary>
        /// Bone rotation and translation update step
        /// </summary>
        private void BoneTransformUpdateStep()
        {
            // for each frame separately
            for (int f = 0; f < inAnim.Frames.Length; f++)
            {
                // for each bone in a frame separately
                for (int b = 0; b < boneCount; b++)
                {
                    double boneWeightSum = ComputeSignificance(b);

                    // if bone is insignificant
                    if (boneWeightSum < sigEpsilon)
                    {
                        // re-initialize bone
                        bool res = ReInitializeBone(b);
                        if (res)
                            continue;
                    }

                    // CoR coordinates
                    Vec3f pstar = new Vec3f();
                    Vec3f qstar = new Vec3f();
                    Dictionary<int, double> boneWeights = outAnim.VertexBoneWeights[b];
                    foreach (int key in boneWeights.Keys)
                    {
                        Vec3f multipV = inAnim.RestPose.Vertices[key].Multiplied((float)(boneWeights[key]* boneWeights[key]));
                        pstar.Add(multipV);

                        // deformation residual
                        Vec3f q = inAnim.RestPose.Vertices[key].Subtracted(RemainingBonesResult(key, b, f));
                        Vec3f multipQ = q.Multiplied((float)boneWeights[key]);
                        qstar.Add(multipQ);
                    }
                    pstar.Divide((float)boneWeightSum);
                    qstar.Divide((float)boneWeightSum);

                    // data matrices P and Q
                    Matrix<double> P = Matrix.Build.Dense(3, inAnim.RestPose.Vertices.Length);
                    Matrix<double> Q = Matrix.Build.Dense(3, inAnim.RestPose.Vertices.Length);
                    for (int i = 0; i < inAnim.RestPose.Vertices.Length; i++)
                    {
                        // vertex pos
                        Vec3f v = inAnim.RestPose.Vertices[i];

                        // deformation residual
                        Vec3f q = v.Subtracted(RemainingBonesResult(i, b, f)); 

                        double weight = 0;
                        if (boneWeights.ContainsKey(i))
                            weight = boneWeights[i];

                        // remove translation
                        Vec3f pNew = v.Subtracted(pstar);
                        P.SetColumn(i, new double[] { weight * pNew.x, weight * pNew.y, weight * pNew.z });

                        Vec3f qNew = q.Subtracted(qstar.Multiplied((float)weight));
                        Q.SetColumn(i, new double[] { qNew.x, qNew.y, qNew.z });
                    }

                    // SVD
                    Matrix<double> m = P * Q.Transpose();
                    var resSvd = m.Svd();
                    Matrix<double> UT = resSvd.U.Transpose();
                    Matrix<double> V = resSvd.VT.Transpose();

                    // find optimum rotation and translation
                    Vector<double> pstarV = Vector.Build.Dense(new double[] { pstar.x, pstar.y, pstar.z });
                    Vector<double> qstarV = Vector.Build.Dense(new double[] { qstar.x, qstar.y, qstar.z });
                    Matrix<double> boneRotation = V * UT;
                    Vector<double> boneTranslation = qstarV - boneRotation * pstarV;

                    // store iun outAnim
                    outAnim.Frames[f].BoneRotation[b] = boneRotation;
                    outAnim.Frames[f].BoneTranslation[b] = boneTranslation;
                }
            }

        }

        /// <summary>
        /// Compute the resulting position of vertex v in frame f if it is transformed by all bones except b
        /// </summary>
        /// <param name="v"> Vertex </param>
        /// <param name="b"> Bone </param>
        /// <param name="f"> Frame </param>
        /// <returns> Alternative position of vertex </returns>
        private Vec3f RemainingBonesResult(int v, int b, int f)
        {
            Frame frame = outAnim.Frames[f];
            Vec3f vertex = inAnim.RestPose.Vertices[v];
            Vector<double> vertexV = Vector.Build.Dense(new double[] { vertex.x, vertex.y, vertex.z });

            Vector<double> sum = Vector.Build.Dense(3);
            for (int i = 0; i < boneCount; i++)
            {
                if (i == b)
                    continue;

                double weight = 0;
                if (outAnim.VertexBoneWeights[b].ContainsKey(v))
                    weight = outAnim.VertexBoneWeights[b][v];

                Vector<double> translation = frame.BoneTranslation[i];
                Matrix<double> rotation = frame.BoneRotation[i];

                sum += weight * (rotation * vertexV + translation);
            }

            Vec3f res = new Vec3f((float)sum[0], (float)sum[1], (float)sum[2]);
            return res;
        }

        /// <summary>
        /// Compute the significance of bone b in animation
        /// </summary>
        /// <param name="b"> Index of bone </param>
        /// <returns> Significance of bone </returns>
        private double ComputeSignificance(int b)
        {
            // go through all weights for bone b, sum of pow
            double res = 0;
            Dictionary<int, double> boneWeights =  outAnim.VertexBoneWeights[b];
            foreach (double value in boneWeights.Values)
                res += value * value;

            return res;
        }

        /// <summary>
        /// Re-initialization of insignificant bone
        /// </summary>
        /// <param name="b"> Bone to re-initialize </param>
        /// <returns> Returns false if unsuccessfull (aka a bone has been re-initialized too many times), true if successfull  </returns>
        private bool ReInitializeBone(int b)
        {
            if (initCount > maxInits)
                return false;

            // find vertex with largest reconstruction error
            double max = double.MinValue;
            int maxIndex = -1;
            Vec3f centroid = new Vec3f();
            for (int i = 0; i < inAnim.RestPose.Vertices.Length; i++)
            {
                double recError = GetVertexReconstructionError(i);
                centroid.Add(inAnim.RestPose.Vertices[i]);

                if (max < recError)
                {
                    max = recError;
                    maxIndex = i;
                }
            }
            centroid.Divide(inAnim.RestPose.Vertices.Length);

            // find 20 nearest vertices to that vertex
            if (tree == null)
                tree = KDTree.BuildTree(inAnim.RestPose.Vertices);
            List<int> neighboursIndices = KDTree.GetNearest(maxIndex, 21, inAnim.RestPose.Vertices, tree);

            // assign bone to that vertex
            for (int i = 0; i < outAnim.VertexBoneWeights.Length; i++)
            {
                for (int j = 0; j < neighboursIndices.Count; j++)
                {
                    if (outAnim.VertexBoneWeights[i].ContainsKey(neighboursIndices[j]))
                        outAnim.VertexBoneWeights[i].Remove(neighboursIndices[j]);
                    outAnim.VertexBoneWeights[b].Add(neighboursIndices[j], 1);
                }
            }

            List<Vec3f> neighbours = new List<Vec3f>();
            for (int i = 0; i < inAnim.RestPose.Vertices.Length; i++)
            {
                Vec3f v = inAnim.RestPose.Vertices[i];
                if (neighboursIndices.Contains(i))
                    neighbours.Add(v);
            }

            Kabsch k = new Kabsch();
            for (int f = 0; f < inAnim.Frames.Length; f++)
            {
                List<Vec3f> neighboursInPose = new List<Vec3f>();

                for (int i = 0; i < inAnim.RestPose.Vertices.Length; i++)
                {
                    Vec3f v = inAnim.Frames[f].Vertices[i];
                    if (neighboursIndices.Contains(i))
                        neighboursInPose.Add(v);
                }

                // re-initialize bone transformation and rotation using Kabsch algorithm
                Matrix<double> rot = k.SolveKabsch(neighbours.ToArray(), neighboursInPose.ToArray());
                outAnim.Frames[f].BoneRotation[b] = rot;
                outAnim.Frames[f].BoneTranslation[b] = k.Translation;
            }

            // increase re-init counter
            initCount++;

            return true;
        }

        /// <summary>
        /// Get reconstruction error of vertex i in animation
        /// </summary>
        /// <param name="i"> Vertex index </param>
        /// <returns> Reconstruction error </returns>
        private double GetVertexReconstructionError(int i)
        {
            double sum = 0;
            Vec3f vRest = inAnim.RestPose.Vertices[i];
            Vector<double> vRestV = Vector.Build.Dense(new double[] { vRest.x, vRest.y, vRest.z });

            // go through all frames
            for (int f = 0; f < inAnim.Frames.Length; f++)
            {
                // get position in frame inAnim
                Vec3f v = inAnim.Frames[f].Vertices[i];

                // reconstruct position in frame outAnim
                Vector<double> posV = Vector.Build.Dense(3);
                for (int b = 0; b < boneCount; b++)
                {
                    double weight = 0;
                    if (outAnim.VertexBoneWeights[b].ContainsKey(i))
                        weight = outAnim.VertexBoneWeights[b][i];

                    Vector<double> translation = outAnim.Frames[f].BoneTranslation[b];
                    Matrix<double> rotation = outAnim.Frames[f].BoneRotation[b];

                    posV += weight * (rotation * vRestV + translation);
                }

                // resulting error in frame
                Vec3f pos = new Vec3f((float)posV[0], (float)posV[1], (float)posV[2]);
                Vec3f resFrame = v.Subtracted(pos);

                // add error to sum
                sum += resFrame.x * resFrame.x + resFrame.y * resFrame.y + resFrame.z * resFrame.z;
            }
         
            return sum;
        }

        private void WeightUpdateStep()
        {
            // FASM algorithm

            throw new NotImplementedException();
        }

        private bool CheckConvergence()
        {
            throw new NotImplementedException();
        }

        private void CorrectRestPose()
        {
            throw new NotImplementedException();
        }
    }
}
