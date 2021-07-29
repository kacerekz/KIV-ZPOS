using MeshAnimation.Animation;
using MeshAnimation.Clustering;
using MeshAnimation.MathUtil;
using MeshAnimation.Util;
using OpenTK;
using OpenTkRenderer;
using OpenTkRenderer.Rendering;
using OpenTkRenderer.Rendering.Materials;
using OpenTkRenderer.Rendering.Meshes;
using OpenTkRenderer.Rendering.Scenes;
using OpenTkRenderer.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// testing push :)

/*
 * Something is weird with the ligting and shadows, either the shadow buffer is just not working or i am forgetting something
 * It might also be unstable - after editing a shader I lost all image & "fixed it" by redoing some edits so no idea what happened there
 */


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

            IAnimation anim = LoadAnimation(animPath);
            GameObject restPose = LoadRestPose(anim);
            restPose.transform *= Matrix4.CreateScale(0.03f);
            // TODO load rest pose -> display w/ colours

            GameObject teapot = LoadObject("Data/teapot.obj");
            GameObject plane = LoadObject("Data/plane.obj");
            plane.transform *= Matrix4.CreateScale(10.0f);
            plane.transform *= Matrix4.CreateTranslation(0.0f, -1.5f, -5.0f);
            teapot.transform *= Matrix4.CreateTranslation(0.0f, -1.5f, -5.0f);

            var light = new Light(
                // position
                new Vector3(5.0f, 5.0f, -5.0f),
                // direction
                new Vector3(-1.0f, -1.0f, 1.0f));

            light.SetParameters(
                // ambient
                new Vector3(1, 1, 1),
                // diffuse
                new Vector3(0.8f, 0.8f, 0.8f),
                // specular
                new Vector3(0.2f, 0.2f, 0.2f));

            light.SetAttenuation(0.09f, 0.032f);
            light.SetCutoff(20f, 30f);

            SceneManager.ActiveScene.activeLights.Add(light);
            SceneManager.ActiveScene.gameObjects.Add("teapot", teapot);
            SceneManager.ActiveScene.gameObjects.Add("plane", plane);
            SceneManager.ActiveScene.gameObjects.Add("restPose", restPose);

            window.Run(config.updatesPerSecond, config.framesPerSecond);
        }

        private static GameObject LoadRestPose(IAnimation anim)
        {
            ObjLoader loader = (ObjLoader)anim.RestPose;

            if (loader.Normals == null || loader.Normals.Length == 0)
                BasicProcessing.CalculateNormals(loader);

            var testMesh = new BasicMesh(loader);

            var vertexShader = new VertexShader(File.ReadAllText("Shaders/colours.vert"));
            var fragmentShader = new FragmentShader(File.ReadAllText("Shaders/colours.frag"));
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
        /// Temporary test method for loading animations
        /// </summary>
        /// <param name="foldername">Path from which to load animation</param>
        /// <returns>Animation</returns>
        private static IAnimation LoadAnimation(string foldername)
        {
            IAnimation anim = new DMAnimation();
            anim.LoadAnimation(foldername);

            KMeans km = new KMeans();
            km.BoneCount = 9;
            km.Cluster((ObjLoader)anim.RestPose);

            // colours according to clusters
            anim.RestPose.Colors = new Vec3f[anim.RestPose.Vertices.Length];
            for (int i = 0; i < km.BoneClusters.Count; i++)
            {
                float step = 1.0f / (km.BoneCount + 1);
                Vec3f color = new Vec3f();
                color.x = step * i;
                color.y = 0.1f;
                color.z = step * i;

                for (int j = 0; j < km.BoneClusters[i].Length; j++)
                    anim.RestPose.Colors[km.BoneClusters[i][j]] = color;
            }

            return anim;
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
