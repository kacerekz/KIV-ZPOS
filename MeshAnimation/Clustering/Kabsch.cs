using MathNet.Numerics.LinearAlgebra;
using OpenTkRenderer.Structs;
using System;

namespace MeshAnimation.Clustering
{
    class Kabsch
    {

        public Matrix<double> Rotation;
        public Vector<double> Translation;

        public Matrix<double> SolveKabsch(Vec3f[] pointSet1, Vec3f[] pointSet2, bool solveRotation = true, bool solveScale = false)
        {
            if (pointSet1.Length != pointSet2.Length)
                return Matrix<double>.Build.DenseIdentity(4);

            // calculate centroids
            Vec3f centroid1 = new Vec3f();
            Vec3f centroid2 = new Vec3f();

           
            for (int i = 0; i < pointSet1.Length; i++)
            {
                centroid1 = new Vec3f(centroid1.x + pointSet1[i].x, centroid1.y + pointSet1[i].y, centroid1.z + pointSet1[i].z);
                centroid2 = new Vec3f(centroid2.x + pointSet2[i].x, centroid2.y + pointSet2[i].y, centroid2.z + pointSet2[i].z);
            }
            centroid1 = new Vec3f(centroid1.x / pointSet1.Length, centroid1.y / pointSet1.Length, centroid1.z / pointSet1.Length);
            centroid2 = new Vec3f(centroid2.x / pointSet1.Length, centroid2.y / pointSet1.Length, centroid2.z / pointSet1.Length);

            Vector<double> centroid1V = Vector<double>.Build.Dense(new double[] { centroid1.x, centroid1.y, centroid1.z });
            Vector<double> centroid2V = Vector<double>.Build.Dense(new double[] { centroid2.x, centroid2.y, centroid2.z });

            // transform centroids to origin
            for (int i = 0; i < pointSet1.Length; i++)
            {
                pointSet1[i].Subtract(centroid1);
                pointSet2[i].Subtract(centroid2);
            }

            // create matrices P and Q
            Matrix<double> P = Matrix<double>.Build.Dense(pointSet1.Length, 3);
            Matrix<double> Q = Matrix<double>.Build.Dense(pointSet1.Length, 3);

            for (int i = 0; i < pointSet1.Length; i++)
            {
                P.SetRow(i, new double[] { pointSet1[i].x, pointSet1[i].y, pointSet1[i].z });
                Q.SetRow(i, new double[] { pointSet2[i].x, pointSet2[i].y, pointSet2[i].z });
            }

            // covariance matrix
            Matrix<double> H = P.Transpose() * Q;

            // svd decomposition
            var svd = H.Svd();
            Matrix<double> UT = svd.U.Transpose();
            Matrix<double> V = svd.VT.Transpose();

            // correction of rotation
            int d = Math.Sign((V * UT).Determinant());
            Matrix<double> diag = Matrix<double>.Build.DenseDiagonal(3, 1);
            diag.SetRow(2, new double[] { 0, 0, d });

            // rotation matrix
            Matrix<double> rotation = V * diag * UT;

            Vector<double> translation = centroid2V - rotation * centroid1V;

            Rotation = rotation;
            Translation = translation;

            return rotation;
        }

    }
}
