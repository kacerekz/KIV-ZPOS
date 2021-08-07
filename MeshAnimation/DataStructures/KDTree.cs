using OpenTkRenderer.Structs;
using Supercluster.KDTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshAnimation.DataStructures
{
    /// <summary>
    /// Using a Supercluster.KDTree to find nearest neighbors
    /// </summary>
    public class KDTree
    {
        /// <summary>
        /// Calculates squared distance of two vectors
        /// </summary>
        private static Func<double[], double[], double> L2Norm = (x, y) =>
        {
            double dist = 0f;
            for (int i = 0; i < x.Length; i++)
            {
                dist += (x[i] - y[i]) * (x[i] - y[i]);
            }
            return dist;
        };

        /// <summary>
        /// Builds a KdTree given the input data array
        /// </summary>
        /// <param name="vertices">Input vertex data array</param>
        /// <returns>Built KdTree</returns>
        public static KDTree<double, int> BuildTree(Vec3f[] vertices)
        {
            // Generate point set for the tree
            double[][] points = new double[vertices.Length][];

            for (int i = 0; i < vertices.Length; i++)
            {
                points[i] = new double[] { vertices[i].x, vertices[i].y, vertices[i].z };
            }

            // Generate node set for the tree
            int[] nodes = new int[vertices.Length];

            for (int i= 0; i < vertices.Length; i++)
            {
                nodes[i] = i;
            }

            // Build the KDTree.
            var tree = new KDTree<double, int>(dimensions: 3, points: points, nodes: nodes, metric: L2Norm);
            return tree;
        }

        /// <summary>
        /// Finds nearest neighborst to a vertex specified by its index into the vertices array.
        /// </summary>
        /// <param name="v">Index into data array</param>
        /// <param name="numNearest">Number of nearest neighbors to find</param>
        /// <param name="vertices">Input data array</param>
        /// <param name="kdTree">KDTree pre-built with the input array</param>
        /// <returns>List of nearest neighbor indices</returns>
        public static List<int> GetNearest(int v, int numNearest, Vec3f[] vertices, KDTree<double, int> kdTree)
        {
            var point = new double[] { vertices[v].x, vertices[v].y, vertices[v].z };
            var result = kdTree.NearestNeighbors(point: point, neighbors: numNearest);

            List<int> nbrs = new List<int>(numNearest);
            foreach (var nbr in result)
                nbrs.Add(nbr.Item2);

            return nbrs;
        }

        /// <summary>
        /// Finds nearest neighborst to a vertex specified by its index into the vertices array.
        /// Prefer the method which uses the cached kdTree if possible.
        /// </summary>
        /// <param name="v">Index into data array</param>
        /// <param name="numNearest">Number of nearest neighbors to find</param>
        /// <param name="vertices">Input data array</param>
        /// <returns>List of nearest neighbor indices</returns>
        public static List<int> GetNearest(int v, int numNearest, Vec3f[] vertices)
        {
            var kdTree = BuildTree(vertices);

            var point = new double[] { vertices[v].x, vertices[v].y, vertices[v].z };
            var result = kdTree.NearestNeighbors(point: point, neighbors: numNearest);

            List<int> nbrs = new List<int>(numNearest);
            foreach (var nbr in result)
                nbrs.Add(nbr.Item2);

            return nbrs;
        }
    }
}
