using MathNet.Numerics.LinearAlgebra;
using MeshAnimation.Animation;
using OpenTkRenderer.Rendering.Meshes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MeshAnimation.Util
{
    class AnimationImporter
    {
        /// <summary> Path to folder the files will be loaded from </summary>
        public static string path = ".";

        /// <summary>
        /// Import a skinned animation
        /// </summary>
        /// <param name="name"> Name of the files </param>
        public static SkinningAnimation ImportSkinningAnimation(string name)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
            SkinningAnimation anim = new SkinningAnimation();

            ObjLoader restPose = ImportObj(name + ".obj");
            anim.RestPose = restPose;

            ImportAnimation(name + ".txt", anim);

            return anim;
        }

        private static void ImportAnimation(string name, SkinningAnimation anim)
        {
            string[] lines = File.ReadAllLines(name);

            bool loadingWeights = false;
            bool loadingFrames = false;

            List<Frame> frames = new List<Frame>();
            List<ConcurrentDictionary<int, double>> weights = new List<ConcurrentDictionary<int, double>>();

            Frame currFrame = null;
            List<Matrix<double>> rotations = null;
            List<Vector<double>> translations = null;

            ConcurrentDictionary<int, double> currWeights = null;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] splitLine = lines[i].Split(' ');

                if (splitLine[0] == "w")
                {
                    loadingWeights = true;

                    if (currWeights != null)
                        weights.Add(currWeights);
                    currWeights = new ConcurrentDictionary<int, double>();
                }
                else if (splitLine[0] == "f")
                {
                    loadingFrames = true;
                    loadingWeights = false;

                    if (currWeights != null)
                    {
                        weights.Add(currWeights);
                        currWeights = null;
                    }

                    if (currFrame != null)
                    {
                        currFrame.BoneRotation = rotations.ToArray();
                        currFrame.BoneTranslation = translations.ToArray();
                        frames.Add(currFrame);
                    }
                    currFrame = new Frame();
                    rotations = new List<Matrix<double>>();
                    translations = new List<Vector<double>>();
                }
                else if (splitLine[0] == "b")
                {
                    double[] matrixD = new double[9];

                    for (int r = 0; r < matrixD.Length; r++)
                        double.TryParse(splitLine[r + 1], out matrixD[r]);

                    Matrix<double> matrix = Matrix<double>.Build.Dense(3, 3);

                    matrix[0, 0] = matrixD[0]; matrix[0, 1] = matrixD[1]; matrix[0, 2] = matrixD[2];
                    matrix[1, 0] = matrixD[3]; matrix[1, 1] = matrixD[4]; matrix[1, 2] = matrixD[5];
                    matrix[2, 0] = matrixD[6]; matrix[2, 1] = matrixD[7]; matrix[2, 2] = matrixD[8];

                    double[] vectorD = new double[3];
                    for (int r = 0; r < vectorD.Length; r++)
                        double.TryParse(splitLine[r + 1 + 9], out vectorD[r]);

                    Vector<double> vector = Vector<double>.Build.Dense(3);

                    vector[0] = vectorD[0]; vector[1] = vectorD[1]; vector[2] = vectorD[2];

                    rotations.Add(matrix);
                    translations.Add(vector);
                }
                else
                {
                    if (loadingWeights)
                    {
                        int index = 0;
                        int.TryParse(splitLine[0], out index);
                        double weight = 0;
                        double.TryParse(splitLine[1], out weight);

                        currWeights.TryAdd(index, weight);
                    }
                }

            }
            currFrame.BoneRotation = rotations.ToArray();
            currFrame.BoneTranslation = translations.ToArray();
            frames.Add(currFrame);

            anim.Frames = frames.ToArray();
            anim.VertexBoneWeights = weights.ToArray();
        }

        private static ObjLoader ImportObj(string name)
        {
            var loader = new ObjLoader();
            loader.Load(name);
            return loader;
        }

    }
}
