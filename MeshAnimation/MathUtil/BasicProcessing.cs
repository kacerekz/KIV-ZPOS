using OpenTkRenderer.Rendering.Meshes;
using OpenTkRenderer.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshAnimation.MathUtil
{
    /// <summary>
    /// Basic mesh processing
    /// </summary>
    public class BasicProcessing
    {
        /// <summary>
        /// Calculates normals
        /// </summary>
        /// <param name="loader">MeshLoader with a mesh already loaded in</param>
        public static void CalculateNormals(MeshLoader loader)
        {
            loader.Normals = new Vec3f[loader.Vertices.Length];

            for (int i = 0; i < loader.Indices.Length; i++)
            {
                var v1 = loader.Vertices[loader.Indices[i].v1];
                var v2 = loader.Vertices[loader.Indices[i].v2];
                var v3 = loader.Vertices[loader.Indices[i].v3];

                var normal = GetNormal(v1, v2, v3);

                loader.Normals[loader.Indices[i].v1].Add(normal);
                loader.Normals[loader.Indices[i].v2].Add(normal);
                loader.Normals[loader.Indices[i].v3].Add(normal);
            }

            for (int i = 0; i < loader.Normals.Length; i++)
            {
                loader.Normals[i].Normalize();
            }
        }

        /// <summary>
        /// Calculates the normal of a face
        /// </summary>
        /// <param name="v1">First vertex</param>
        /// <param name="v2">Second vertex</param>
        /// <param name="v3">Third vertex</param>
        /// <returns>The normal of the face specified by v1, v2 and v3</returns>
        private static Vec3f GetNormal(Vec3f v1, Vec3f v2, Vec3f v3)
        {
            var a = v2.Clone();
            var b = v3.Clone();
            a.Subtract(v1);
            b.Subtract(v1);
            return a.Crossed(b);
        }
    }
}
