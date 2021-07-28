using OpenTkRenderer.Rendering.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshAnimation.Clustering
{
    interface IClustering
    {
        List<int[]> BoneClusters { get; }
        MeshLoader[] Animation { get; set; }

        bool Cluster(ObjLoader restPose);
    }
}
