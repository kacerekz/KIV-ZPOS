using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace OpenTkRenderer.Rendering
{
    /// <summary>
    /// Uniform info
    /// </summary>
    public struct UniformInfo
    {
        /// <summary>
        /// Uniform name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Uniform location
        /// </summary>
        public int Location { get; set; }

        /// <summary>
        /// Uniform size
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Uniform type
        /// </summary>
        public ActiveUniformType Type { get; set; }
    }
}
