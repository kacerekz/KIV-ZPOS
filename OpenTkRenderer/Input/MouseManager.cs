using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Input
{
    /// <summary>
    /// Publishes mouse state
    /// </summary>
    class MouseManager
    {
        /// <summary>
        /// Attached mouse observers
        /// </summary>
        private static List<IMouseObserver> observers;

        /// <summary>
        /// Last mouse coordinates
        /// </summary>
        private static int lastX, lastY;

        /// <summary>
        /// OpenTK window
        /// </summary>
        public static GameWindow Window { get; set; }


        /// <summary>
        /// Init MouseManager
        /// </summary>
        static MouseManager()
        {
            observers = new List<IMouseObserver>();
        }

        /// <summary>
        /// Updates keyboard state
        /// </summary>
        public static void Update()
        {
            var mouse = Mouse.GetCursorState();
            var deltaX = mouse.X - lastX;
            var deltaY = mouse.Y - lastY;
            CenterMouse();

            Notify(deltaX, deltaY);
        }

        /// <summary>
        /// Notifies observers of keyboard state
        /// </summary>
        public static void Notify(float deltaX, float deltaY)
        {
            foreach (var observer in observers)
            {
                observer.Update(deltaX, deltaY);
            }
        }

        /// <summary>
        /// Attach an observer
        /// </summary>
        /// <param name="observer">A mouse observer</param>
        public static void Attach(IMouseObserver observer)
        {
            observers.Add(observer);
        }

        /// <summary>
        /// Detach an observer
        /// </summary>
        /// <param name="observer">A mouse observer</param>
        public static void Detach(IMouseObserver observer)
        {
            observers.Remove(observer);
        }

        /// <summary>
        /// Center the cursor on the screen
        /// </summary>
        private static void CenterMouse()
        {
            var center = Window.PointToScreen(new System.Drawing.Point(Window.Width / 2, Window.Height / 2));
            Mouse.SetPosition(center.X, center.Y);

            lastX = center.X;
            lastY = center.Y;
        }
    }
}
