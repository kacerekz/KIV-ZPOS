using MeshAnimation.Animation;
using MeshAnimation.Optimization;
using MeshAnimation.Scenes;
using MeshAnimation.Util;

using OpenTK;
using OpenTkRenderer;
using OpenTkRenderer.Rendering;
using OpenTkRenderer.Rendering.Scenes;
using OpenTkRenderer.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// testing push :)

namespace MeshAnimation
{
    /// <summary>
    /// Mesh animation and processing coursework for KIV/ZPOS 2020/21.
    /// </summary>
    class Program
    {
        public static Config config;

        /// <summary>
        /// Depending on the mode, the program processes or renders mesh animations
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            var c = new Config();
            Serialization.Serialize<Config>(c, "./config.xml");

            var pathToConfigFile = (args.Length == 0) ? "./config.xml" : args[0];
            
            config = File.Exists(pathToConfigFile)
                ? Serialization.Deserialize<Config>(pathToConfigFile)
                : new Config();

            switch (config.mode)
            {
                case Mode.Process:
                    Process(config);
                    break;
                case Mode.Render:
                    Render(config);
                    break;
            }
        }

        /// <summary>
        /// Run in render mode
        /// </summary>
        /// <param name="config">Settings</param>
        private static void Process(Config config)
        {
            var window = new OpenTkWindow(config.windowWidth, config.windowHeight, "MeshAnimation");

            // This is where all the objects get added to the scene
            // DemoScene.CreateScene(animPath);
            SkinningAnimation a = OptimizeAnimation(config.path);
            AnimationScene.CreateScene(a, config.scaleModel);

            window.Run(config.updatesPerSecond, config.framesPerSecond);
        }

        private static SkinningAnimation OptimizeAnimation(string foldername)
        {
            // Load animation
            IAnimation anim = new DMAnimation();
            anim.LoadAnimation(foldername);

            SSDROptimizer op = new SSDROptimizer();
            op.boneCount = config.boneCount;
            op.maxIterations = config.ssdrIterations;
            op.sigEpsilon = config.toleranceForReinit;
            op.significantBoneCount = config.significantBoneCount;
            op.maxIterationsGen = config.geneticIterations;
            op.populationSizeGen = config.generationSize;
            op.neighCount = config.neighbourCount;

            SkinningAnimation res = op.Optimize(anim, config.geneticAlgorithm);

            // Export animation
            AnimationExporter.ExportSkinningAnimation(res, config.outName);
            res = AnimationImporter.ImportSkinningAnimation(config.outName);
            
            // Add colours of the mosst significant bone to each vertex
            Random r = new Random();
            res.RestPose.Colors = new Vec3f[res.RestPose.Vertices.Length];
            double[] w = new double[res.RestPose.Vertices.Length];
            for (int i = 0; i < w.Length; i++) w[i] = double.MinValue;
            for (int i = 0; i < op.boneCount; i++)
            {
                float step = 1.0f / (op.boneCount + 1);
                Vec3f color = new Vec3f();
                color.x = step * i;
                color.y = (float)r.NextDouble();
                color.z = (float)r.NextDouble();

                foreach (int v in res.VertexBoneWeights[i].Keys)
                {
                    if (res.VertexBoneWeights[i][v] > w[v])
                    {
                        res.RestPose.Colors[v] = color;
                        w[v] = res.VertexBoneWeights[i][v];
                    }

                }
            }

            return res;
        }

        /// <summary>
        /// Run in processing mode
        /// </summary>
        /// <param name="config">Settings</param>
        private static void Render(Config config)
        {
            var window = new OpenTkWindow(config.windowWidth, config.windowHeight, "MeshAnimation");

            // Load animation from file
            SkinningAnimation a = AnimationImporter.ImportSkinningAnimation(config.path);

            // Add colours of the mosst significant bone to each vertex
            Random r = new Random();
            a.RestPose.Colors = new Vec3f[a.RestPose.Vertices.Length];
            double[] w = new double[a.RestPose.Vertices.Length];
            for (int i = 0; i < w.Length; i++) w[i] = double.MinValue;
            for (int i = 0; i < a.VertexBoneWeights.Length; i++)
            {
                float step = 1.0f / (a.VertexBoneWeights.Length + 1);
                Vec3f color = new Vec3f();
                color.x = step * i;
                color.y = (float)r.NextDouble();
                color.z = (float)r.NextDouble();

                foreach (int v in a.VertexBoneWeights[i].Keys)
                {
                    if (a.VertexBoneWeights[i][v] > w[v])
                    {
                        a.RestPose.Colors[v] = color;
                        w[v] = a.VertexBoneWeights[i][v];
                    }

                }
            }

            AnimationScene.CreateScene(a, config.scaleModel);

            window.Run(config.updatesPerSecond, config.framesPerSecond);
        }

    }
}
