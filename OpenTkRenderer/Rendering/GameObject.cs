using OpenTK;
using OpenTkRenderer.Rendering.Materials;
using OpenTkRenderer.Rendering.Meshes;
using OpenTkRenderer.Rendering.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Rendering
{
    /// <summary>
    /// Base GameObject class
    /// </summary>
    public class GameObject
    {
        /// <summary>
        /// GameObject will only render if it is not disabled
        /// </summary>
        public bool disabled;

        /// <summary>
        /// GameObject representation
        /// </summary>
        public Mesh mesh;

        /// <summary>
        /// GameObject material
        /// </summary>
        public Material material = Material.NONE;

        /// <summary>
        /// Model matrix of the object
        /// </summary>
        public Matrix4 transform = Matrix4.Identity;

        /// <summary>
        /// Renders the game object
        /// </summary>
        /// <param name="scene">The parent scene</param>
        public void Render(Scene scene)
        {
            if (disabled) return;

            material.Set(scene, transform);
            mesh.Render();
            material.Clear();
        }
    }
}
