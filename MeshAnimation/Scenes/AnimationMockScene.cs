using MathNet.Numerics.LinearAlgebra;
using MeshAnimation.Animation;
using OpenTkRenderer.Rendering.Meshes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshAnimation.Scenes
{
    /// <summary>
    /// Animation test scene
    /// </summary>
    public class AnimationMockScene : AnimationScene
    {

        /// <summary>
        /// Create a scene using a mock animation
        /// </summary>
        /// <param name="animationPath">Path to files to animate (only first will be used)</param>
        public static void CreateScene(string animationPath)
        {
            CreateScene(GenerateMockAnimation(animationPath));
        }

        /// <summary>
        /// Generates the animation
        /// </summary>
        /// <param name="animationPath">Path to files to animate (only first will be used)</param>
        /// <returns>Mock animation</returns>
        public static SkinningAnimation GenerateMockAnimation(string animationPath)
        {
            var files = Directory.EnumerateFiles(animationPath).ToArray();
            var loader = new ObjLoader();
            loader.Load(files[0]);

            int boneCount = 2;
            int frameCount = 360;

            var animation = new SkinningAnimation(loader, boneCount, frameCount);

            // Find min and max Y coordinates, lerp for bone weight
            // Assign weights to bone 0 of the animation
            double minY = double.MaxValue;
            double maxY = double.MinValue;
            double weight;
            double span;
            double y;

            var M = Matrix<double>.Build;
            var V = Vector<double>.Build;

            double shiftY = 250f / frameCount;
            double rotY = (360 * 4 / frameCount) * 0.01745; // radians

            for (int i = 0; i < loader.Vertices.Length; i++)
            {
                y = loader.Vertices[i].y;
                if (y < minY) minY = y;
                if (y > maxY) maxY = y;
            }

            span = maxY - minY;

            for (int i = 0; i < loader.Vertices.Length; i++)
            {
                y = loader.Vertices[i].y;
                weight = (y - minY) / span;
                animation.VertexBoneWeights[0].Add(i, weight);
                animation.VertexBoneWeights[1].Add(i, 1 - weight);
            }

            // Now generate a translation and a rotation for each frame
            for (int i = 0; i < frameCount; i++)
            {
                animation.Frames[i].BoneTranslation[0] = V.Dense(new double[] {0, i < frameCount / 2 ? i * shiftY : (frameCount - i) * shiftY, 0});
                animation.Frames[i].BoneRotation[0] = CreateRotationY(M, i * rotY);
             
                animation.Frames[i].BoneTranslation[1] = V.Dense(new double[] {0, 0, 0});
                animation.Frames[i].BoneRotation[1] = CreateRotationY(M, 0);
            }

            return animation;
        }

        /// <summary>
        /// Creates a Y axis rotation matrix
        /// </summary>
        /// <param name="M">Matrix builder</param>
        /// <param name="rotY">Angle in radians by which to rotate</param>
        /// <returns>Y axis rotation matrix</returns>
        private static Matrix<double> CreateRotationY(MatrixBuilder<double> M, double rotY)
        {
            var rot = M.DenseIdentity(3);

            double cos = Math.Cos(rotY);
            double sin = Math.Sin(rotY);

            rot[0,0] = cos;
            rot[0,2] = sin;
            rot[2,0] = -sin;
            rot[2,2] = cos;

            return rot;
        }
    
    }
}
