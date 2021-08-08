using MeshAnimation.Animation;
using MeshAnimation.Scenes;
using MeshAnimation.Util;

using OpenTK;
using OpenTkRenderer;
using OpenTkRenderer.Rendering;
using OpenTkRenderer.Rendering.Scenes;

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
        static string animPath = @"D:\moje\school\04\zpos\ZPOS data\constant connectivity\jump";

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
            //DemoScene.CreateScene(animPath);
            AnimationMockScene.CreateScene(animPath);

            window.Run(config.updatesPerSecond, config.framesPerSecond);
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
