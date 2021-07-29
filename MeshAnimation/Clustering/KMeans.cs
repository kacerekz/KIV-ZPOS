using System;
using System.Collections.Generic;
using OpenTkRenderer.Rendering.Meshes;
using OpenTkRenderer.Structs;

namespace MeshAnimation.Clustering
{
    /// <summary>
    /// K-means clustering
    /// </summary>
    class KMeans : IClustering
    {
        /// <summary> Number of clusters </summary>
        public int BoneCount;

        /// <summary> Created clusters </summary>
        List<int[]> boneClusters;
        public List<int[]> BoneClusters { get => boneClusters; }

        /// <summary>
        /// Cluster objFile into BoneCount clusters
        /// </summary>
        /// <param name="objFile"> Rest pose </param>
        /// <returns> True if successfull, false if not </returns>
        public bool Cluster(ObjLoader objFile)
        {
            if (BoneCount <= 0 || objFile == null)
                return false;

            // call clustering library on data
            Vec3f[] vertices = objFile.Vertices;
            if (vertices == null || vertices.Length == 0)
                return false;

            KMeansCluster(vertices);
            
            return true;
        }

        /// <summary>
        /// Clustering library call and processing result
        /// </summary>
        /// <param name="vertices"> Input data </param>
        private void KMeansCluster(Vec3f[] vertices)
        {
            boneClusters = new List<int[]>();

            // cluster
            KMeansResults res = KMeansLib.Cluster(vertices, BoneCount, 2, 0);
            
            // clusters into BoneClusters
            for (int i = 0; i < res.clusters.Length; i++)
            {
                boneClusters.Add(res.clusters[i]);
                Console.WriteLine(res.clusters[i].Length);
            }
        }
    }
}
