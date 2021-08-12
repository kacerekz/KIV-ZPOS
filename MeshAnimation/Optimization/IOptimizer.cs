using MeshAnimation.Animation;
using System;

namespace MeshAnimation.Optimization
{
    interface IOptimizer
    {

        /// <summary>
        /// Create a skinning animation from input animated mesh sequence
        /// </summary>
        /// <param name="anim"></param>
        /// <returns></returns>
        SkinningAnimation Optimize(IAnimation inAnim, bool geneticAlgorithm = false);

    }
}
