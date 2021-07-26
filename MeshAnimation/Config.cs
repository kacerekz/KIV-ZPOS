using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshAnimation
{
    /// <summary>
    /// Program configuration
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Mode to launch in
        /// </summary>
        public Mode mode = Mode.Render;

        /// <summary>
        /// Window update frequency
        /// </summary>
        public int updatesPerSecond = 100;

        /// <summary>
        /// Window FPS cap
        /// </summary>
        public int framesPerSecond = 100;

        /// <summary>
        /// Window update frequency
        /// </summary>
        public int windowWidth = 800;

        /// <summary>
        /// Window FPS cap
        /// </summary>
        public int windowHeight = 600;


    }
}
