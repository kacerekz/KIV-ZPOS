using OpenTkRenderer.Rendering.Meshes;
using System.Collections.Generic;

namespace MeshAnimation.Clustering
{
    /// <summary>
    /// Clustering interface
    /// </summary>
    interface IClustering
    {
        /// <summary>
        /// Result clusters
        /// </summary>
        List<int[]> BoneClusters { get; }

        /// <summary>
        /// Cluster call
        /// </summary>
        /// <param name="objFile"> Input file </param>
        /// <returns> True if successfull, false if not </returns>
        bool Cluster(ObjLoader objFile);
    }
}
