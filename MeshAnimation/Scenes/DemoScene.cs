using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using MeshAnimation.Animation;
using MeshAnimation.Clustering;
using MeshAnimation.MathUtil;

using OpenTkRenderer.Rendering;
using OpenTkRenderer.Rendering.Materials;
using OpenTkRenderer.Rendering.Meshes;
using OpenTkRenderer.Rendering.Scenes;
using OpenTkRenderer.Structs;

namespace MeshAnimation.Scenes
{
    /// <summary>
    /// A demo scene showing object loading and clustering results
    /// </summary>
    public class DemoScene : Scene
    {
        /// <summary>
        /// Create the demo scene showing object loading and clustering results
        /// </summary>
        /// <param name="path">path to animation</param>
        public static void CreateScene(string path)
        {
            // loading animation
            IAnimation anim = LoadAnimation(path);
            GameObject restPose = LoadRestPose(anim);
            restPose.transform *= Matrix4.CreateScale(0.03f); // jump 0.03 scale, samba 1 scale
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
        }

        /// <summary>
        /// Temporary test method - load rest pose of animation into a game object
        /// </summary>
        /// <param name="anim"> Input animation </param>
        /// <returns> Rest pose game object </returns>
        private static GameObject LoadRestPose(IAnimation anim)
        {
            ObjLoader loader = (ObjLoader)anim.RestPose;

            if (loader.Normals == null || loader.Normals.Length == 0)
                MeshProcessing.CalculateNormals(loader);

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
        /// Temporary test method - load animation
        /// </summary>
        /// <param name="foldername">Path from which to load animation</param>
        /// <returns>Animation</returns>
        private static IAnimation LoadAnimation(string foldername)
        {
            // load animation
            IAnimation anim = new DMAnimation();
            anim.LoadAnimation(foldername);

            // cluster
            KMeans km = new KMeans();
            km.BoneCount = 9;

            ObjLoader[] objs = new ObjLoader[anim.Frames.Length];
            for (int f = 0; f < anim.Frames.Length; f++)
                objs[f] = (ObjLoader)anim.Frames[f];

            km.Cluster(objs);

            Random r = new Random();
            // colours according to clusters
            anim.RestPose.Colors = new Vec3f[anim.RestPose.Vertices.Length];
            for (int i = 0; i < km.BoneClusters.Count; i++)
            {
                float step = 1.0f / (km.BoneCount + 1);
                Vec3f color = new Vec3f();
                color.x = step * i;
                color.y = (float)r.NextDouble();
                color.z = (float)r.NextDouble();

                for (int j = 0; j < km.BoneClusters[i].Length; j++)
                    anim.RestPose.Colors[km.BoneClusters[i][j]] = color;
            }

            return anim;
        }

    }
}
