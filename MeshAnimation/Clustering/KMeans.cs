using System;
using System.Collections.Generic;
using OpenTkRenderer.Rendering.Meshes;

namespace MeshAnimation.Clustering
{
    class KMeans : IClustering
    {
        public int BoneCount;

        List<int[]> boneClusters;
        public List<int[]> BoneClusters { get => boneClusters; }

        MeshLoader[] animation;
        public MeshLoader[] Animation { get => animation; set => animation = value; }

        public bool Cluster(MeshLoader restPose)
        {
            if (BoneCount <= 0)
                return false;

            // call clustering library

            throw new NotImplementedException();
        }
    }
}
