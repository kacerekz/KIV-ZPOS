using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Rendering.Cameras
{
    /// <summary>
    /// The settings of a camera
    /// </summary>
    public class CameraSettings
    {
        /// <summary>
        /// Default camera settingd
        /// </summary>
        public static CameraSettings DEFAULT = new CameraSettings(0.1f, 100f, 40f);

        /// <summary>
        /// Near plane of the camera
        /// </summary>
        public float Near { get; set; }

        /// <summary>
        /// Far plane of the camera
        /// </summary>
        public float Far { get; set; }

        /// <summary>
        /// Horizontal field of view
        /// </summary>
        public float HFOV { get; set; }

        /// <summary>
        /// Creates camera settings
        /// </summary>
        /// <param name="near">Near plane of the camera</param>
        /// <param name="far">Far plane of the camera</param>
        /// <param name="hFOV">Horizontal field of view</param>
        public CameraSettings(float near, float far, float hFOV)
        {
            Near = near;
            Far = far;
            HFOV = (float)Math.Tan(hFOV * Math.PI / 180);
        }
    
    }
}
