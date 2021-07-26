using MeshAnimation.Util;
using OpenTkRenderer;
using OpenTkRenderer.Rendering;
using OpenTkRenderer.Rendering.Materials;
using OpenTkRenderer.Rendering.Meshes;
using OpenTkRenderer.Rendering.Scenes;
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

        /// <summary>
        /// Run in render mode
        /// </summary>
        /// <param name="config">Settings</param>
        private static void Render(Config config)
        {
            var window = new OpenTkWindow(config.windowWidth, config.windowHeight, "MeshAnimation");

            var loader = new ObjLoader();
            loader.Load("Data/teapot.obj");
            var testMesh = new BasicMesh(loader);

            var vertexShader = new VertexShader(File.ReadAllText("Shaders/basic.vert"));
            var fragmentShader = new FragmentShader(File.ReadAllText("Shaders/basic.frag"));
            var testShader = new ShaderProgram(vertexShader, fragmentShader);

            var testMaterial = new BasicMaterial();
            testMaterial.shaderProgram = testShader;

            var testObject = new GameObject();
            testObject.mesh = testMesh;
            testObject.material = testMaterial;
            testObject.transform = OpenTK.Matrix4.Identity;
            testObject.transform *= OpenTK.Matrix4.CreateTranslation(0, 0, -10f);

            SceneManager.ActiveScene.gameObjects.Add("testMesh", testObject);

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
