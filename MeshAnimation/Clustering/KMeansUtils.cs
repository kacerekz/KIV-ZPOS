﻿using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MeshAnimation.Clustering;
using OpenTkRenderer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;

// TODO předávat si průměry -> aby se počítalo dycky se stejnejma


/// <summary>
/// Provides a simple implementation of the k-Means algorithm. This solution is quite simple and does not support any parallel execution as of yet.
/// </summary>
public static class KMeansLib 
{
    static Kabsch kabschSolver;
    public static Matrix<double>[][] tMatrices;
    public static Vector<double>[][] tVectors;

    public static KMeansResults Cluster(Vec3f[][] items, int clusterCount, int maxIterations, int seed) {
        kabschSolver = new Kabsch();

        double[][][] data = new double[items.Length][][];
        for (int f = 0; f < items.Length; f++) {
            data[f] = new double[items[f].Length][];
            for (int i = 0; i < items[f].Length; i++) {
                Vec3f v = items[f][i];
                data[f][i] = new double[] { v.x, v.y, v.z };
            }
        }
        return Cluster(data, clusterCount, maxIterations, seed);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"> [frame][vertex][coordinates] </param>
    /// <param name="clusterCount"></param>
    /// <param name="maxIterations"></param>
    /// <param name="seed"></param>
    /// <returns></returns>
    public static KMeansResults Cluster(double[][][] data, int clusterCount, int maxIterations, int seed) {

        bool hasChanges = true;
        int iteration = 0;
        double totalDistance = 0;
        int frameCount = data.Length;
        int numData = data[0].Length;
        int numAttributes = data[0][0].Length;

        // Create a random initial clustering assignment - same for all frames
        int[] clustering = InitializeClustering(numData, clusterCount, seed);

        // Create cluster means and centroids
        double[][][] means = CreateMatrix(frameCount, clusterCount, numAttributes);
        int[,] centroidIdx = new int[frameCount,clusterCount];

        tMatrices = CreateMatrices(frameCount, clusterCount);
        tVectors = CreateVectors(frameCount, clusterCount);

        int[,] clusterItemCount = new int[frameCount, clusterCount]; // TODO OPTI - same for all frames -> store only once

        // Perform the clustering - need to take into account all frames
        while (hasChanges && iteration < maxIterations) {
            clusterItemCount = new int[frameCount, clusterCount];
            totalDistance = CalculateClusteringInformation(data, clustering, ref means, ref centroidIdx, clusterCount, ref clusterItemCount);
            hasChanges = AssignClustering(data, means, clustering, centroidIdx, clusterCount);
            ++iteration;
        }

        // Create the final clusters
        List<int>[] clusters = new List<int>[clusterCount];
        for (int k = 0; k < clusters.Length; k++)
            clusters[k] = new List<int>();

        int[] clustersCurIdx = new int[clusterCount];
        for (int i = 0; i < clustering.Length; i++)
            clusters[clustering[i]].Add(i);

        Console.WriteLine(clustering.Length + " vs " + data[0].Length);

        int[][] realclusters = new int[clusterCount][];
        for (int k = 0; k < clusterCount; k++)
            realclusters[k] = clusters[k].ToArray();

        // Return the results
        return new KMeansResults(realclusters); // , means, centroidIdx, totalDistance);
    }

    private static Matrix<double>[][] CreateMatrices(int frameCount, int clusterCount)
    {
        Matrix<double>[][] array = new Matrix<double>[frameCount][];
        for (int f = 0; f < frameCount; f++)
        {
            array[f] = new Matrix<double>[clusterCount];
            for (int i = 0; i < clusterCount; i++)
                array[f][i] = Matrix.Build.DenseIdentity(3);
        }

        return array;
    }

    private static Vector<double>[][] CreateVectors(int frameCount, int clusterCount)
    {
        Vector<double>[][] array = new Vector<double>[frameCount][];
        for (int f = 0; f < frameCount; f++)
        {
            array[f] = new Vector<double>[clusterCount];
            for (int i = 0; i < clusterCount; i++)
                array[f][i] = Vector<double>.Build.Dense(3);
        }

        return array;
    }

    private static int[] InitializeClustering(int numData, int clusterCount, int seed) {

        var rnd = new System.Random(seed);
        var clustering = new int[numData];

        for (int i = 0; i < numData; ++i)
            clustering[i] = rnd.Next(0, clusterCount);

        return clustering;
    }

    private static double[][][] CreateMatrix(int depth, int rows, int columns) {
        var matrix = new double[depth][][];

        for (int f = 0; f < matrix.Length; f++)
        {
            matrix[f] = new double[rows][];

            for (int i = 0; i < matrix[f].Length; i++)
                matrix[f][i] = new double[columns];
        }

        return matrix;
    }

    private static double CalculateClusteringInformation(double[][][] data, int[] clustering, ref double[][][] means, ref int[,] centroidIdx, int clusterCount, ref int[,] clusterItemCount)
    {

        // get sets of points
        // in each frame is bones sets
        // compute rotation matrix between them
        // for each frame for each bone compute rotation between rest pose and frame set of points

        // rest pose points
        List<Vec3f>[] clusterDataRP = new List<Vec3f>[clusterCount];
        for (int i = 0; i < clusterCount; i++)
            clusterDataRP[i] = new List<Vec3f>();

        for (int j = 0; j < data[0].Length; j++)
        {
            int index = clustering[j];
            clusterDataRP[index].Add(new Vec3f((float)data[0][j][0], (float)data[0][j][1], (float)data[0][j][2]));
        }

        for (int f = 0; f < data.Length; f++)
        {
            // data in frame in cluster
            List<Vec3f>[] clusterData = new List<Vec3f>[clusterCount];
            for (int i = 0; i < clusterCount; i++)
                clusterData[i] = new List<Vec3f>();

            // file data into lists based on clusters
            for (int j = 0; j < data[f].Length; j++)
            {
                int index = clustering[j];
                clusterData[index].Add(new Vec3f((float)data[f][j][0], (float)data[f][j][1], (float)data[f][j][2]));
            }

            // compute optimal rotation matrix
            for (int i = 0; i < clusterCount; i++)
            {
                tMatrices[f][i] = kabschSolver.SolveKabsch(clusterDataRP[i].ToArray(), clusterData[i].ToArray());
                tVectors[f][i] = kabschSolver.Translation;
            }
        }


        // Reset the means to zero for all clusters in all frames
        foreach (var frame in means)
        {
            foreach (var mean in frame)
                for (int i = 0; i < mean.Length; i++)
                    mean[i] = 0;
        }

        // Calculate the means for each cluster
        // Do this in two phases, first sum them all up and then divide by the count in each cluster
        for (int f = 0; f < data.Length; f++)
        {
            for (int i = 0; i < data[f].Length; i++)
            {
                // Sum up the means
                var row = data[f][i];
                var clusterIdx = clustering[i]; // What cluster is data i assigned to
                ++clusterItemCount[f, clusterIdx]; // Increment the count of the cluster that row i is assigned to
                for (int j = 0; j < row.Length; j++)
                    means[f][clusterIdx][j] += row[j];
            }
        }

        // Now divide to get the average
        for (int f = 0; f < means.Length; f++)
        {
            for (int k = 0; k < means[f].Length; k++)
            {
                for (int a = 0; a < means[f][k].Length; a++)
                {
                    int itemCount = clusterItemCount[f,k];
                    means[f][k][a] /= itemCount > 0 ? itemCount : 1;
                }
            }
        }

        double totalDistance = 0;
        // Calc the centroids
        double[] minDistances = new double[clusterCount].Select(x => double.MaxValue).ToArray();
        for (int i = 0; i < data[0].Length; i++)
        {
            var clusterIdx = clustering[i]; // What cluster is data i assigned to
            var distance = 0.0;
            for (int f = 0; f < data.Length; f++)
                distance += CalculateDistance(data[f][i], means[f][clusterIdx]);

            totalDistance += distance;
            if (distance < minDistances[clusterIdx])
            {
                minDistances[clusterIdx] = distance;
                
                for (int f = 0; f < data.Length; f++)
                    centroidIdx[f,clusterIdx] = i;
            }
        }

        return totalDistance;
    }

    /// <summary>
    /// Calculates the distance for each point in <see cref="data"/> from each of the centroid in <see cref="centroidIdx"/> and 
    /// assigns the data item to the cluster with the minimum distance.
    /// </summary>
    /// <returns>true if any clustering arrangement has changed, false if clustering did not change.</returns>
    private static bool AssignClustering(double[][][] data, double[][][] means, int[] clustering, int[,] centroidIdx, int clusterCount)
    {
        bool changed = false;

        // compare transformation of rest pose vertices to position of vertices in frame
        // squared distance between those
        // add together thorough all frames


        for (int i = 0; i < data[0].Length; i++) {
            double minDistance = double.MaxValue;
            int minClusterIndex = -1;

            for (int k = 0; k < clusterCount; k++) {

                double distance = 0;
                for (int f = 0; f < data.Length; f++)
                {
                    // Vector<double> resPos = tMatrices[f][k] * Vector<double>.Build.Dense(new double[] { data[0][i][0] - means[0][k][0], data[0][i][1] - means[0][k][1], data[0][i][2] - means[0][k][2] });
                    // double[] arrayRes = new double[] { resPos[0] + means[f][k][0], resPos[1] + means[f][k][1], resPos[2] + means[f][k][2] };

                    Vector<double> resPos = tMatrices[f][k] * Vector<double>.Build.Dense(new double[] { data[0][i][0], data[0][i][1], data[0][i][2]}) + tVectors[f][k];
                    double[] arrayRes = new double[] { resPos[0], resPos[1], resPos[2] };

                    distance += CalculateDistance(data[f][i], arrayRes);

                    // distance += CalculateDistance(data[f][i], data[f][centroidIdx[f, k]]);
                }

                if (distance < minDistance) {
                    minDistance = distance;
                    minClusterIndex = k;
                }
            }

            // Re-arrange the clustering for datapoint if needed
            if (minClusterIndex != -1 && clustering[i] != minClusterIndex) {
                changed = true;
                clustering[i] = minClusterIndex;
            }
        }

        return changed;
    }

    /// <summary>
    ///  Calculates the eculidean distance from the <see cref="point"/> to the <see cref="centroid"/>
    /// </summary>
    private static double CalculateDistance(double[] point, double[] centroid) {
        // For each attribute calculate the squared difference between the centroid and the point
        double sum = 0;
        for (int i = 0; i < point.Length; i++)
            sum += Math.Pow(centroid[i] - point[i], 2);

        return Math.Sqrt(sum);
    }
}


/// <summary>
/// Represents a single result from the <see cref="KMeans"/> algorithm. 
/// Contains the original items arranged into the clusters converged on as well as the centroids chosen and the total distance of the converged solution.
/// </summary>
/// <typeparam name="T"></typeparam>
public class KMeansResults {

    /// <summary>
    /// The original items arranged into the clusters converged on
    /// </summary>
    public readonly int[][] clusters;

    /// <summary>
    /// The final mean values used for the clusters. Mostly for debugging purposes.
    /// </summary>
    public readonly double[][] means;

    /// <summary>
    /// The list of centroids used in the final solution. These are indicies into the original data.
    /// </summary>
    public readonly int[] centroids;

    /// <summary>
    /// The total distance between all the nodes and their centroids in the final solution. 
    /// This can be used as a reference point on how "good" the solution is when the algorithm is run repeatedly with different starting configuration.
    /// Lower is "usually" better.
    /// </summary>
    public readonly double totalDistance;

    public KMeansResults(int[][] clusters, double[][] means, int[] centroids, double totalDistance) {
        this.clusters = clusters;
        this.means = means;
        this.centroids = centroids;
        this.totalDistance = totalDistance;
    }


    public KMeansResults(int[][] clusters)
    {
        this.clusters = clusters;
    }
}
