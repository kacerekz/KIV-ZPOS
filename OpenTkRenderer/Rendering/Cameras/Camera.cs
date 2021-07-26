using OpenTK;
using OpenTkRenderer.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Rendering.Cameras
{
    public class Camera : IInputObserver
    {
        /// <summary>
        /// Camera settings
        /// </summary>
        public CameraSettings settings;

        /// <summary>
        /// Projection matrix
        /// </summary>
        public Matrix4 projection;

        /// <summary>
        /// View matrix
        /// </summary>
        public Matrix4 view;

        /// <summary>
        /// Update based on keyboard input
        /// </summary>
        /// <param name="state">Keyboard state</param>
        public virtual void Update(IDictionary<string, bool> state) { }

        // Add a MouseObserver
    }
}
