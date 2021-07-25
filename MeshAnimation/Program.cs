using MeshAnimation.Util;
using OpenTkRenderer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshAnimation
{
    /// <summary>
    /// Mesh animation and processing coursework for KIV/ZPOS 2020/21.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Depending on the mode, the program processes or renders mesh animations
        /// </summary>
        /// <param name="args">Command line arguments</param>
        static void Main(string[] args)
        {
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

        private static void Render(Config config)
        {
            var window = new OpenTkWindow(config.windowWidth, config.windowHeight, "MeshAnimation");
            window.Run(config.updatesPerSecond, config.framesPerSecond);
        }

        private static void Process(Config config)
        {
            Console.WriteLine("Processing mode not implemented.");
        }
    }
}
