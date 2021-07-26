using MeshAnimation.MathUtil;
using MeshAnimation.Util;
using OpenTK;
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
            GameObject teapot = LoadObject("Data/teapot.obj");
            GameObject plane = LoadObject("Data/plane.obj");
            plane.transform *= Matrix4.CreateScale(10.0f);
            plane.transform *= Matrix4.CreateTranslation(0.0f, -1.5f, -5.0f);
            teapot.transform *= Matrix4.CreateTranslation(0.0f, -1.5f, -5.0f);

            var light = new Light(
                // position
                new Vector3(0.0f, 3.0f, -5.0f),
                // direction
                new Vector3(0.0f, -1.0f, 0.0f));

            light.SetParameters(
                // ambient
                new Vector3(0.0f, 0.1f, 0.1f),
                // diffuse
                new Vector3(1.0f, 1.0f, 1.0f),
                // specular
                new Vector3(0.2f, 0.2f, 0.2f));

            light.SetAttenuation(0.09f, 0.032f);
            light.SetCutoff(20f, 30f);

            SceneManager.ActiveScene.activeLights.Add(light);
            SceneManager.ActiveScene.gameObjects.Add("teapot", teapot);
            SceneManager.ActiveScene.gameObjects.Add("plane", plane);

            window.Run(config.updatesPerSecond, config.framesPerSecond);
        }

        /// <summary>
        /// Temporary test method for loading objects
        /// </summary>
        /// <param name="filename">Path from which to load a mesh</param>
        /// <returns>GameObject represented by mesh at the given path</returns>
        private static GameObject LoadObject(string filename)
        {
            var loader = new ObjLoader();
            loader.Load(filename);
            if (loader.Normals == null || loader.Normals.Length == 0)
                BasicProcessing.CalculateNormals(loader);

            var testMesh = new BasicMesh(loader);

            var vertexShader = new VertexShader(File.ReadAllText("Shaders/basic.vert"));
            var fragmentShader = new FragmentShader(File.ReadAllText("Shaders/basic.frag"));
            var testShader = new ShaderProgram(vertexShader, fragmentShader);

            var testMaterial = new BasicMaterial();
            testMaterial.shaderProgram = testShader;

            var testObject = new GameObject();
            testObject.mesh = testMesh;
            testObject.material = testMaterial;
            testObject.transform = Matrix4.Identity;
            return testObject;
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
