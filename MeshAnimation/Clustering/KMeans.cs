using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
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

        public Matrix<double>[][] tMatrices;
        public Vector<double>[][] tVectors;

        /// <summary>
        /// Cluster objFile into BoneCount clusters
        /// </summary>
        /// <param name="objFile"> Rest pose </param>
        /// <returns> True if successfull, false if not </returns>
        public bool Cluster(ObjLoader[] objFiles)
        {
            if (BoneCount <= 0 || objFiles == null || objFiles.Length == 0)
                return false;

            // call clustering library on data
            Vec3f[][] vertices = new Vec3f[objFiles.Length][];
            for (int i = 0; i < objFiles.Length; i++)
            {
                vertices[i] = objFiles[i].Vertices;
                if (vertices[i] == null)
                    return false;
            }

            KMeansCluster(vertices);
            
            return true;
        }

        /// <summary>
        /// Clustering library call and processing result
        /// </summary>
        /// <param name="vertices"> Input data </param>
        private void KMeansCluster(Vec3f[][] vertices)
        {
            boneClusters = new List<int[]>();

            // cluster
            KMeansResults res = KMeansLib.Cluster(vertices, BoneCount, 5, 0);
            
            // clusters into BoneClusters
            for (int i = 0; i < res.clusters.Length; i++)
            {
                boneClusters.Add(res.clusters[i]);
                // Console.WriteLine(res.clusters[i].Length);
            }

            tVectors = KMeansLib.tVectors;
            tMatrices = KMeansLib.tMatrices;
        }
    }
}
