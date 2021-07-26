using OpenTkRenderer.Rendering.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Rendering
{
    /// <summary>
    /// A single render pass
    /// </summary>
    public class RenderPass
    {
        /// <summary>
        /// Action before render
        /// </summary>
        Action beginPass = () => { };

        /// <summary>
        /// Action after render
        /// </summary>
        Action endPass = () => { };

        /// <summary>
        /// An empty constructor
        /// </summary>
        public RenderPass() { }

        /// <summary>
        /// A render pass with a pre-render action
        /// </summary>
        /// <param name="beginPass">Action before render</param>
        public RenderPass(Action beginPass)
        {
            this.beginPass = beginPass;
        }

        /// <summary>
        /// Render with actions before and after it
        /// </summary>
        /// <param name="beginPass">Action before render</param>
        /// <param name="endPass">Action after render</param>
        public RenderPass(Action beginPass, Action endPass)
        {
            this.beginPass = beginPass;
            this.endPass = endPass;
        }

        /// <summary>
        /// Render a scene
        /// </summary>
        /// <param name="scene">Scene to be rendered</param>
        public virtual void Render(Scene scene)
        {
            beginPass();
            foreach (var gameObject in scene.GetActiveGameObjects())
            {
                gameObject.Render(scene);
            }
            endPass();
        }

    }
}
