using System;
using System.Collections.Generic;
using OpenTkRenderer.Rendering.Meshes;
using OpenTkRenderer.Structs;

namespace MeshAnimation.Clustering
{
    class KMeans : IClustering
    {
        public int BoneCount;

        List<int[]> boneClusters;
        public List<int[]> BoneClusters { get => boneClusters; }

        MeshLoader[] animation;
        public MeshLoader[] Animation { get => animation; set => animation = value; }

        public bool Cluster(ObjLoader restPose)
        {
            if (BoneCount <= 0)
                return false;

            // call clustering library on this data
            Vec3f[] vertices = restPose.Vertices;

            KMeansCluster(vertices);

            return true;
        }

        private void KMeansCluster(Vec3f[] vertices)
        {
            boneClusters = new List<int[]>();

            KMeansResults res = KMeansLib.Cluster(vertices, BoneCount, 2, 0);
            for (int i = 0; i < res.clusters.Length; i++)
            {
                boneClusters.Add(res.clusters[i]);
                Console.WriteLine(res.clusters[i].Length);
            }
        }
    }
}
