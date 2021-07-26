using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Rendering.Scenes
{
    /// <summary>
    /// Manages scene continuity and switching
    /// </summary>
    public class SceneManager
    {
        /// <summary>
        /// Currently active scene
        /// </summary>
        public static Scene ActiveScene { get; set; }

        /// <summary>
        /// Current render pass settings
        /// </summary>
        public static RenderPass RenderPass { get; set; }

        /// <summary>
        /// Initialize defaults
        /// </summary>
        static SceneManager()
        {
            ActiveScene = new Scene();
            RenderPass = new RenderPass();
        }
    }
}
