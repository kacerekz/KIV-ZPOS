using MeshAnimation.Animation;
using MeshAnimation.Clustering;
using OpenTkRenderer.Rendering.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshAnimation.Optimization
{
    class SSDROptimizer : IOptimizer
    {
        public int boneCount = 9;
        public int maxIterations = 1000;

        /// <summary>
        /// Optimize animation
        /// </summary>
        /// <returns></returns>
        public SkinningAnimation Optimize(IAnimation inAnim)
        {
            // TODO two different paths for TVM and DMA

            SkinningAnimation outAnim = new SkinningAnimation((ObjLoader)inAnim.RestPose, boneCount, inAnim.Frames.Length);

            // initialize
            InitializationStep(inAnim, outAnim);

            // start loop
            int iteration = 0;
            while(true)
            {
                // update weights
                WeightUpdateStep();
                // update bone transformations
                BoneTransformUpdateStep();

                CheckForInsignificantBones(outAnim);

                // check if converged
                if (CheckConvergence(outAnim) || iteration > maxIterations)
                    break;

                iteration++;
            }

            CorrectRestPose(outAnim);

            return outAnim;
        }

        private void CheckForInsignificantBones(SkinningAnimation outAnim)
        {
            throw new NotImplementedException();
        }

        private void CorrectRestPose(SkinningAnimation outAnim)
        {
            throw new NotImplementedException();
        }

        private void BoneTransformUpdateStep()
        {
            throw new NotImplementedException();
        }

        private void WeightUpdateStep()
        {
            throw new NotImplementedException();
        }

        private bool CheckConvergence(SkinningAnimation outAnim)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialization step
        /// </summary>
        /// <param name="inAnim"> Input animated mesh sequence </param>
        /// <param name="outAnim"> Output skinning animation </param>
        private void InitializationStep(IAnimation inAnim, SkinningAnimation outAnim)
        {
            // clustering
            KMeans km = Cluster(inAnim);
            // clustering into weight map
            PrepareOutAnimation(inAnim, outAnim, km);
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
        private void PrepareOutAnimation(IAnimation inAnim, SkinningAnimation outAnim, KMeans km)
        {
            // set rest pose
            outAnim.RestPose = (ObjLoader)inAnim.RestPose;

            // set weight map
            for (int i = 0; i < km.BoneClusters.Count; i++)
                for (int j = 0; j < km.BoneClusters[i].Length; j++)
                    outAnim.VertexBoneWeights[km.BoneClusters[i][j]].Add(i, 1);
        }
    }
}
