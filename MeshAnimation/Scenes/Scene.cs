using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using MeshAnimation.MathUtil;
using OpenTkRenderer.Rendering;
using OpenTkRenderer.Rendering.Materials;
using OpenTkRenderer.Rendering.Meshes;

namespace MeshAnimation.Scenes
{
    /// <summary>
    /// Scene base class. Objects are added to scenes. Scenes also create scene-specific key mappings in this application.
    /// </summary>
    public abstract class Scene
    {
        /// <summary>
        /// Temporary test method for loading objects
        /// </summary>
        /// <param name="filename">Path from which to load a mesh</param>
        /// <returns>GameObject represented by mesh at the given path</returns>
        protected static GameObject LoadObject(string filename, string vertexShaderPath = "Shaders/basic.vert", string fragmentShaderPath = "Shaders/basic.frag")
        {
            var loader = new ObjLoader();
            loader.Load(filename);
            return LoadObject(loader, vertexShaderPath, fragmentShaderPath);
        }

        /// <summary>
        /// Temporary test method for loading objects
        /// </summary>
        /// <param name="loader">Loader with a preloaded mesh</param>
        /// <returns>GameObject represented by mesh at the given path</returns>
        protected static GameObject LoadObject(MeshLoader loader, string vertexShaderPath = "Shaders/basic.vert", string fragmentShaderPath = "Shaders/basic.frag")
        {
            if (loader.Normals == null || loader.Normals.Length == 0)
                MeshProcessing.CalculateNormals(loader);

            var testMesh = new BasicMesh(loader);

            var vertexShader = new VertexShader(File.ReadAllText(vertexShaderPath));
            var fragmentShader = new FragmentShader(File.ReadAllText(fragmentShaderPath));
            var testShader = new ShaderProgram(vertexShader, fragmentShader);

            var testMaterial = new BasicMaterial();
            testMaterial.shaderProgram = testShader;

            var testObject = new GameObject();
            testObject.mesh = testMesh;
            testObject.material = testMaterial;
            testObject.transform = Matrix4.Identity;
            return testObject;
        }
    }
}
