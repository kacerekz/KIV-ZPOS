using OpenTK;
using OpenTkRenderer.Rendering.Scenes;
using System;

namespace OpenTkRenderer.Rendering.Materials
{
    /// <summary>
    /// Material base class
    /// </summary>
    public class Material
    {
        /// <summary>
        /// Empty material preset
        /// </summary>
        public static Material NONE = new Material();

        /// <summary>
        /// Sets material parameters
        /// </summary>
        /// <param name="scene">Parent scene data</param>
        /// <param name="model">Model matrix of the object being rendered</param>
        public virtual void Set(Scene scene, Matrix4 model) { }

        /// <summary>
        /// Clears material
        /// </summary>
        public virtual void Clear() { }
    }
}