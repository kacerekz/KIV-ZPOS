using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Input
{
    /// <summary>
    /// Interface of a mouse input observer
    /// </summary>
    interface IMouseObserver
    {
        /// <summary>
        /// Update after mouse state is observed
        /// </summary>
        /// <param name="deltaX">Mouse coordinate delta in X axis</param>
        /// <param name="deltaY">Mouse coordinate delta in Y axis</param>
        void Update(float deltaX, float deltaY);
    }
}
