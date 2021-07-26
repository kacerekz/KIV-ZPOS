using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTkRenderer.Structs;

namespace OpenTkRenderer.Rendering.Meshes
{
    /// <summary>
    /// Mesh loader template to be extended by implementations for specific 3D file types
    /// </summary>
    public abstract class MeshLoader
    {
        /// <summary>
        /// Vertex array
        /// </summary>
        public Vec3f[] Vertices { get; internal set; }

        /// <summary>
        /// Normals array
        /// </summary>
        public Vec3f[] Normals { get; set; }
        
        /// <summary>
        /// Color array
        /// </summary>
        public Vec3f[] Colors { get; set; }
        
        /// <summary>
        /// Texture coordinate array
        /// </summary>
        public Vec2f[] TexCoords { get; set; }

        /// <summary>
        /// Index array
        /// </summary>
        public Vec3i[] Indices { get; internal set; }


        /// <summary>
        /// Load the mesh
        /// </summary>
        /// <param name="filename">Path to mesh file</param>
        public abstract void Load(string filename);
    }
}
