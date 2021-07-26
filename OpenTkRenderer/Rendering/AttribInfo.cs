using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace OpenTkRenderer.Rendering
{
    /// <summary>
    ///  Attribute info
    /// </summary>
    public struct AttribInfo
    {
        /// <summary>
        /// Attribute name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Attribute location
        /// </summary>
        public int Location { get; set; }

        /// <summary>
        /// Attribute size
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Attribute type
        /// </summary>
        public ActiveAttribType Type { get; set; }
    }
}
