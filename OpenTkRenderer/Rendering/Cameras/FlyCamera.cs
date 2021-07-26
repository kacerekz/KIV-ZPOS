using OpenTK;
using OpenTkRenderer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Rendering.Cameras
{
    /// <summary>
    /// Classic camera
    /// </summary>
    public class FlyCamera : Camera
    {
        /// <summary>
        /// Camera system
        /// </summary>
        public Vector3 up, forward, position;

        /// <summary>
        /// Yaw amount
        /// </summary>
        public float yaw;

        /// <summary>
        /// Pitch amount
        /// </summary>
        public float pitch;

        /// <summary>
        /// Camera base speed before FPS scaling
        /// </summary>
        public float baseSpeed;

        /// <summary>
        /// Creates a camera with default settings
        /// </summary>
        /// <param name="position">Initial position</param>
        /// <param name="forward">Forward vector</param>
        public FlyCamera(Vector3 position, Vector3 forward) : this(position, forward, CameraSettings.DEFAULT) { }

        /// <summary>
        /// Creates a camera with given values
        /// </summary>
        /// <param name="position">Initial position</param>
        /// <param name="forward">Forward vector</param>
        /// <param name="settings">Camera settings</param>
        public FlyCamera(Vector3 position, Vector3 forward, CameraSettings settings)
        {
            this.settings = settings;
            this.position = position;
            this.forward = forward;
            this.up = new Vector3(0, 1, 0);

            this.yaw = (float)Math.Atan2(forward.Z, forward.X);
            this.pitch = 0;

            this.baseSpeed = 0.1f;

            UpdateView();
        }

        /// <summary>
        /// Recalculates the view matrix based on the yaw and pitch values
        /// </summary>
        private void UpdateView()
        {
            Vector3 forwardNew;
            forwardNew.X = (float)(Math.Cos(yaw) * Math.Cos(pitch));
            forwardNew.Y = (float)Math.Sin(pitch);
            forwardNew.Z = (float)(Math.Sin(yaw) * Math.Cos(pitch));
            forward = forwardNew;

            view = Matrix4.LookAt(position, position + forward, up);
        }

        /// <summary>
        /// Updates camera position based on keyboard input
        /// </summary>
        /// <param name="state">Keyboard state</param>
        public override void Update(IDictionary<string, bool> state)
        {
            Vector3 oldPosition = position;

            var speed = baseSpeed * Time.deltaTime;

            if (state.ContainsKey("MoveForward") && state["MoveForward"])
                position += speed * forward;

            if (state.ContainsKey("MoveBackwards") && state["MoveBackwards"])
                position -= speed * forward;

            if (state.ContainsKey("MoveUp") && state["MoveUp"])
                position += speed * up;

            if (state.ContainsKey("MoveDown") && state["MoveDown"])
                position -= speed * up;

            if (state.ContainsKey("MoveLeft") && state["MoveLeft"])
                position -= Vector3.Normalize(Vector3.Cross(forward, up)) * speed;

            if (state.ContainsKey("MoveRight") && state["MoveRight"])
                position += Vector3.Normalize(Vector3.Cross(forward, up)) * speed;

            Vector3 posTmp = new Vector3(position.X, 0, position.Z);
            if (posTmp.Length > 45) position = oldPosition;

            UpdateView();
        }

        /// <summary>
        /// Update camera tilt based on mouse delta
        /// </summary>
        /// <param name="deltaX">Mouse delta on X axis</param>
        /// <param name="deltaY">Mouse delta on Y axis</param>
        public override void Update(float deltaX, float deltaY)
        {
            Yaw(deltaX * Time.deltaTime * 0.001f);
            Pitch(-deltaY * Time.deltaTime * 0.001f);
        }

        /// <summary>
        /// Changes the yaw based on delta since last frame
        /// </summary>
        /// <param name="delta">Yaw difference since last frame</param>
        public void Yaw(float delta)
        {
            yaw += delta;
            UpdateView();
        }

        /// <summary>
        /// Changes the pitch based on delta since last frame
        /// </summary>
        /// <param name="delta">Pitch difference since last frame</param>
        public void Pitch(float delta)
        {
            pitch += delta;

            if (pitch < MathHelper.DegreesToRadians(-89)) pitch = MathHelper.DegreesToRadians(-89);
            if (pitch > MathHelper.DegreesToRadians(89)) pitch = MathHelper.DegreesToRadians(89);

            UpdateView();
        }
    }
}
