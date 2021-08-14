using MeshAnimation.Animation;
using OpenTkRenderer.Rendering.Meshes;
using System.Globalization;
using System.IO;

namespace MeshAnimation.Util
{
    class AnimationExporter
    {

        /// <summary>
        /// Export skinned animation
        /// </summary>
        /// <param name="anim"> Animation to export </param>
        /// <param name="name"> Path to the output files, without extensions </param>
        public static void ExportSkinningAnimation(SkinningAnimation anim, string name)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);

            ExportObj(anim.RestPose, name);
            ExportAnimation(anim, name);
        }

        private static void ExportAnimation(SkinningAnimation anim, string name)
        {
            string fileData = "";

            // weights
            for (int b = 0; b < anim.VertexBoneWeights.Length; b++)
            {
                fileData += "w\n";
                var weights = anim.VertexBoneWeights[b];
                foreach (int key in weights.Keys)
                {
                    fileData += key + " " + weights[key] + "\n";
                }
            }
            
            int counter = 0;

            // transformations
            for (int f = 0; f < anim.Frames.Length; f++)
            {
                Frame frame = anim.Frames[f];
                fileData += "f\n";

                for (int b = 0; b < frame.BoneTranslation.Length; b++)
                {
                    fileData += "b ";
                    // rotation matrix
                    fileData += frame.BoneRotation[b][0, 0] + " " + frame.BoneRotation[b][0, 1] + " " + frame.BoneRotation[b][0, 2] + " " +
                                frame.BoneRotation[b][1, 0] + " " + frame.BoneRotation[b][1, 1] + " " + frame.BoneRotation[b][1, 2] + " " +
                                frame.BoneRotation[b][2, 0] + " " + frame.BoneRotation[b][2, 1] + " " + frame.BoneRotation[b][2, 2] + " ";
                    // translation
                    fileData += frame.BoneTranslation[b][0] + " " + frame.BoneTranslation[b][1] + " " + frame.BoneTranslation[b][2] + "\n";
                }

                counter++;
            }

            File.WriteAllText(name + ".txt", fileData);
        }

        private static void ExportObj(ObjLoader restPose, string name)
        {
            string fileData = "";

            for (int i = 0; i < restPose.Vertices.Length; i++)
                fileData += "v " + restPose.Vertices[i].x + " " + restPose.Vertices[i].y + " " + restPose.Vertices[i].z + "\n";

            for (int i = 0; i < restPose.Indices.Length; i++)
                fileData += "f " + (restPose.Indices[i].v1 + 1) + " " + (restPose.Indices[i].v2 + 1) + " " + (restPose.Indices[i].v3 + 1) + "\n";

            File.WriteAllText(name + ".obj", fileData);
        }
    }
}
