using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Structs
{
    /// <summary>
    /// Represents position and color data
    /// </summary>
    public struct PositionColor
    {
        /// <summary>
        /// Position data
        /// </summary>
        public float x, y, z;

        /// <summary>
        /// Color data
        /// </summary>
        float r, g, b;

        /// <summary>
        /// Sets position data
        /// </summary>
        /// <param name="x">X axis position</param>
        /// <param name="y">Y axis position</param>
        /// <param name="z">Z axis position</param>
        /// <param name="r">Red color</param>
        /// <param name="g">Green color</param>
        /// <param name="b">Blue color</param>
        public PositionColor(float x, float y, float z, float r, float g, float b)
        {
            this.x = x;
            this.y = y;
            this.z = z;

            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
}
