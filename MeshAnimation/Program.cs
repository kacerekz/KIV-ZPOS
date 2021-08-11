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
        // TODO path to animation - debug ONLY! CHANGE IN YOUR APP!!
        static string animPath = @"D:\moje\school\04\zpos\ZPOS data\constant connectivity\test2";

        /// <summary>
        /// Depending on the mode, the program processes or renders mesh animations
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            var pathToConfigFile = (args.Length == 0) ? "config.xml" : args[0];
            
            var config = File.Exists(pathToConfigFile)
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
        private static void Render(Config config)
        {
            var window = new OpenTkWindow(config.windowWidth, config.windowHeight, "MeshAnimation");

            // This is where all the objects get added to the scene
            // DemoScene.CreateScene(animPath);
            SkinningAnimation a = OptimizeAnimation(animPath);
            AnimationScene.CreateScene(a);

            window.Run(config.updatesPerSecond, config.framesPerSecond);
        }

        private static SkinningAnimation OptimizeAnimation(string foldername)
        {
            // load animation
            IAnimation anim = new DMAnimation();
            anim.LoadAnimation(foldername);

            SSDROptimizer op = new SSDROptimizer();
            SkinningAnimation res = op.Optimize(anim);


            // TODO here adding colours to restpose
            // Add meshes
            Random r = new Random();
            // colours according to clusters
            res.RestPose.Colors = new Vec3f[res.RestPose.Vertices.Length];
            double[] w = new double[res.RestPose.Vertices.Length];
            for (int i = 0; i < w.Length; i++)
                w[i] = double.MinValue;

            for (int i = 0; i < op.boneCount; i++)
            {
                float step = 1.0f / (op.boneCount + 1);
                Vec3f color = new Vec3f();
                color.x = step * i;
                color.y = (float)r.NextDouble();
                color.z = (float)r.NextDouble();

                Console.WriteLine(color.x + " " + color.y + " " + color.z);

                foreach (int v in res.VertexBoneWeights[i].Keys)
                {
                    if (res.VertexBoneWeights[i][v] > w[v])
                    {
                        anim.RestPose.Colors[v] = color;
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
        private static void Process(Config config)
        {
            Console.WriteLine("Processing mode not implemented.");
        }

    }
}
