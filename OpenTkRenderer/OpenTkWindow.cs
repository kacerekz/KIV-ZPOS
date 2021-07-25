using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using OpenTkRenderer.Input;

namespace OpenTkRenderer
{
    /// <summary>
    /// An OpenTK window
    /// </summary>
    public class OpenTkWindow : GameWindow, IInputObserver
    {
        /// <summary>
        /// Minimum OpenGL version to run
        /// </summary>
        private static Version minVersion = new Version(3, 3);

        /// <summary>
        /// Minimum GLSL version to run
        /// </summary>
        private static Version glslMinVersion = new Version(4, 2);

        /// <summary>
        /// Default graphics mode
        /// </summary>
        private static GraphicsMode graphicsMode = new GraphicsMode(new ColorFormat(32), 24, 8, 16);


        /// <summary>
        /// Test for OpenGL version
        /// Create the app window
        /// </summary>
        /// <param name="width">Window width</param>
        /// <param name="height">Window height</param>
        /// <param name="mode">Window graphics mode</param>
        /// <param name="title">Window title</param>
        public OpenTkWindow(int width, int height, string title)
        : base(width, height, graphicsMode, title, GameWindowFlags.Default)
        {
            // Test for OpenGL version
            string versionString = GL.GetString(StringName.Version);
            string glslString = GL.GetString(StringName.ShadingLanguageVersion);

            Console.WriteLine($"OpenGL required: {minVersion.Major}.{minVersion.Minor}");
            Console.WriteLine($"GLSL required: {glslMinVersion.Major}.{glslMinVersion.Minor}");

            Console.WriteLine(versionString);
            Console.WriteLine(glslString);

            int major = int.Parse("" + versionString[0]);
            int minor = int.Parse("" + versionString[2]);
            Version version = new Version(major, minor);

            int glslMajor = int.Parse("" + glslString[0]);
            int glslMinor = int.Parse("" + glslString[2]);
            Version glslVersion = new Version(glslMajor, glslMinor);

            Console.WriteLine($"OpenGL verison: {major}.{minor}");
            Console.WriteLine($"GLSL verison: {glslMajor}.{glslMinor}");

            if (version < minVersion)
            {
                Console.WriteLine("OpenGL version not supported.");
                Exit();
            }
            else if (glslVersion < glslMinVersion)
            {
                Console.WriteLine("GLSL version not supported.");
                Exit();
            }

            Location = new Point(0, 0);
            CursorVisible = false;
        }

        /// <summary>
        /// Update window according to keyboard state
        /// </summary>
        /// <param name="state">Keyboard state</param>
        public void Update(IDictionary<string, bool> state)
        {
            if (state["Exit"]) Exit();
        }

        /// <summary>
        /// Execute when the window is loaded
        /// </summary>
        /// <param name="e">Event data</param>
        protected override void OnLoad(EventArgs e)
        {
            Context.SwapInterval = 0;

            GL.ClearColor(1, 0, 0, 1);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.DrawBuffer(DrawBufferMode.Back);

            InputManager.Attach(this);
        }

        /// <summary>
        /// Execute when the window is resized
        /// </summary>
        /// <param name="e">Event data</param>
        protected override void OnResize(EventArgs e)
        {
            GL.MatrixMode(MatrixMode.Projection);
            GL.Viewport(0, 0, Width, Height);
            GL.LoadIdentity();

            //Scene s = SceneManager.ActiveScene();

            //float far = s.camera.settings.Far;
            //float near = s.camera.settings.Near;
            //float horiz = s.camera.settings.HFOV;
            //float vert = horiz * Height / Width;

            //GL.Frustum(-near * horiz, near * horiz, -near * vert, near * vert, near, far);
            //s.camera.projection = Matrix4.CreatePerspectiveOffCenter(-near * horiz, near * horiz, -near * vert, near * vert, near, far);

            base.OnResize(e);
        }

        /// <summary>
        /// Execute on frame update
        /// </summary>
        /// <param name="e">Frame event</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (!Focused) return;

            InputManager.UpdateState();

            // Time.Update();
            // FPSCounter.Update();
            // Title = $"{FPSCounter.GetFPS()} FPS";
            //Scene s = SceneManager.ActiveScene();
            //var mouse = Mouse.GetCursorState();
            //s.camera.Yaw((float)((mouse.X - mouseLastX) * e.Time));
            //s.camera.Pitch((float)(-(mouse.Y - mouseLastY) * e.Time));
            //CenterMouse();

            base.OnUpdateFrame(e);
        }

        /// <summary>
        /// Execute when a frame is rendering
        /// </summary>
        /// <param name="e">Event data</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        /// <summary>
        /// Execute when the window is closing
        /// </summary>
        /// <param name="e">Event data</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

    }
}
