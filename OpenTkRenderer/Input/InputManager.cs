using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace OpenTkRenderer.Input
{
    /// <summary>
    /// Manages keyboard mapping and publishes keyboard state
    /// </summary>
    class InputManager
    {
        /// <summary>
        /// Keyboard mapping
        /// </summary>
        private static IDictionary<string, Key> mapping;

        /// <summary>
        /// Keyboard state
        /// </summary>
        private static IDictionary<string, bool> state;

        /// <summary>
        /// Attached keyboard observers
        /// </summary>
        private static List<IInputObserver> observers;

        /// <summary>
        /// Initialize instances and default mapping
        /// </summary>
        static InputManager()
        {
            mapping = new Dictionary<string, Key>();
            state = new Dictionary<string, bool>();
            observers = new List<IInputObserver>();
            CreateKeyMapping();
        }

        /// <summary>
        /// Creates default key mapping
        /// </summary>
        public static void CreateKeyMapping()
        {
            mapping["Exit"] = Key.Escape;
        }

        /// <summary>
        /// Updates keyboard state
        /// </summary>
        public static void UpdateState()
        {
            var inputs = mapping.Keys;
            var keyboardState = Keyboard.GetState();

            foreach (var input in inputs)
            {
                state[input] = keyboardState.IsKeyDown(mapping[input]);
            }

            Notify();
        }

        /// <summary>
        /// Notifies observers of keyboard state
        /// </summary>
        public static void Notify()
        {
            foreach (var observer in observers)
            {
                observer.Update(state);
            }
        }

        /// <summary>
        /// Attach an observer
        /// </summary>
        /// <param name="observer">A keyboard observer</param>
        public static void Attach(IInputObserver observer)
        {
            observers.Add(observer);
        }

        /// <summary>
        /// Detach an observer
        /// </summary>
        /// <param name="observer">A keyboard observer</param>
        public static void Detach(IInputObserver observer)
        {
            observers.Remove(observer);
        }
    }
}
