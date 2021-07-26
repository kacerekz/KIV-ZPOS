using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Rendering
{
    /// <summary>
    /// Represents a light and its parameters
    /// </summary>
    public class Light
    {
        /// <summary>
        /// Light position
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// Light direction
        /// </summary>
        public Vector3 direction;

        /// <summary>
        /// Light parameter strengths
        /// </summary>
        public Vector3 ambient, diffuse, specular;

        /// <summary>
        /// Light attenuation parameters
        /// </summary>
        public float linear, quadratic;

        /// <summary>
        /// Light cutoff parameters
        /// </summary>
        public float cutoff, outerCutoff;

        /// <summary>
        /// Create a with a given direction at a given position
        /// </summary>
        /// <param name="position">Light position</param>
        /// <param name="direction">Light direction</param>
        public Light(Vector3 position, Vector3 direction)
        {
            this.position = position;
            this.direction = direction;
            this.direction.Normalize();
        }

        /// <summary>
        /// Set light parameters
        /// </summary>
        /// <param name="ambient"></param>
        /// <param name="diffuse"></param>
        /// <param name="specular"></param>
        public void SetParameters(Vector3 ambient, Vector3 diffuse, Vector3 specular)
        {
            this.ambient = ambient;
            this.diffuse = diffuse;
            this.specular = specular;
        }

        /// <summary>
        /// Set attenuation parameters
        /// </summary>
        /// <param name="constant">Constant multiplier</param>
        /// <param name="linear">Linear multiplier</param>
        /// <param name="quadratic">Quadratic multiplier</param>
        public void SetAttenuation(float linear, float quadratic)
        {
            this.linear = linear;
            this.quadratic = quadratic;
        }

        /// <summary>
        /// Set spotlight cutoff parameters in degrees
        /// </summary>
        /// <param name="cutoff">Inner cutoff</param>
        /// <param name="outerCutoff">Outer cutoff</param>
        public void SetCutoff(float cutoff, float outerCutoff)
        {
            this.cutoff = MathHelper.DegreesToRadians(cutoff);
            this.outerCutoff = MathHelper.DegreesToRadians(outerCutoff);
        }

        /// <summary>
        /// Creates the view matrix of a spotlight
        /// (as if it were a camera)
        /// </summary>
        /// <returns>View matrix of spotlight</returns>
        public Matrix4 BuildViewMatrix()
        {
            return Matrix4.LookAt(position, position + direction, new Vector3(direction.Y, -direction.X, 0));
        }

        /// <summary>
        /// Creates the projecton matrix of a light
        /// </summary>
        /// <returns>Projection matrix of light</returns>
        public Matrix4 BuildProjectionMatrix()
        {
            float noLightIntensity = 0.01f;
            float a = noLightIntensity * quadratic;
            float b = noLightIntensity * linear;
            float c = noLightIntensity * 1 - 1;

            float maxDistance = (float)((-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a));

            return Matrix4.CreatePerspectiveFieldOfView(2 * outerCutoff, 1, 0.1f, maxDistance);
        }

    }
}
