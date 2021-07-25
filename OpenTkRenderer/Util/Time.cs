using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Util
{
    /// <summary>
    /// Tracks time since application launch
    /// </summary>
    class Time
    {
        /// <summary>
        /// Stopwatch instance initialized in a static constructor
        /// </summary>
        private static Stopwatch stopwatch;

        /// <summary>
        /// Time of last update
        /// </summary>
        private static long lastTime;

        /// <summary>
        /// Difference in time between current and last update
        /// </summary>
        public static long deltaTime;

        /// <summary>
        /// Static initialization of stopwatch used to track time since application launch
        /// </summary>
        static Time()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        /// <summary>
        /// Time since application launch
        /// </summary>
        /// <returns>Time since application launch in milliseconds</returns>
        public static long CurrentTime()
        {
            return stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Tracks time difference since last update
        /// </summary>
        public static void Update()
        {
            deltaTime = stopwatch.ElapsedMilliseconds - lastTime;
            lastTime = stopwatch.ElapsedMilliseconds;
        }

    }
}
