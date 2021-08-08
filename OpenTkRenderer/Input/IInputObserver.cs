using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Input
{
    /// <summary>
    /// Interface of a keyboard input observer
    /// </summary>
    public interface IInputObserver
    {
        /// <summary>
        /// Update after keyboard state is observed
        /// </summary>
        /// <param name="state">Keyboard state</param>
        void Update(IDictionary<string, bool> state);
    }
}
