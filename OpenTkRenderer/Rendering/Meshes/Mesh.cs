using System;

namespace OpenTkRenderer.Rendering.Meshes
{
    /// <summary>
    /// A GameObject mesh representation
    /// </summary>
    public abstract class Mesh
    {
        /// <summary>
        /// Render mesh
        /// </summary>
        public abstract void Render();

        /// <summary>
        /// Destroy mesh
        /// </summary>
        public abstract void Destroy();
    }
}