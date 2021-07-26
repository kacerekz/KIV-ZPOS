using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Util
{
    /// <summary>
    /// FPS calculation
    /// </summary>
    class FPSCounter
    {
        /// <summary>
        /// Time since last FPS update
        /// </summary>
        private static long deltaTimeSum;

        /// <summary>
        /// Frames since last FPS update
        /// </summary>
        private static int frameCount;

        /// <summary>
        /// Calculated fps
        /// </summary>
        private static int fps;

        /// <summary>
        /// Updates FPS stats
        /// </summary>
        public static void Update()
        {
            deltaTimeSum += Time.deltaRenderTime;
            frameCount++;

            if (deltaTimeSum > 1000)
            {
                fps = (int)((1000.0 * frameCount) / (deltaTimeSum));
                deltaTimeSum = 0;
                frameCount = 0;
            }
        }

        /// <summary>
        /// Returns FPS
        /// </summary>
        /// <returns>Frames per second</returns>
        public static int GetFPS()
        {
            return fps;
        }

        /// <summary>
        /// Writes FPS to console title
        /// </summary>
        public static void PrintFPS()
        {
            Console.Title = $"{GetFPS()} FPS";
        }
    }
}
