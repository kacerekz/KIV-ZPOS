using OpenTkRenderer.Rendering.Meshes;
using System;
using System.Collections.Generic;

namespace MeshAnimation.Animation
{
    /// <summary>
    /// Output skinning animation
    /// </summary>
    class SkinningAnimation
    {
        /// <summary> Number of bones </summary>
        int boneCount;

        /// <summary> Rest pose </summary>
        ObjLoader restPose;
        public ObjLoader RestPose { get => restPose; set => restPose = value; }
        
        /// <summary> Bone-vertex weights - array of dictionaries for bones, key is vertex index, value is weight </summary>
        Dictionary<int, double>[] vertexBoneWeights;
        public Dictionary<int, double>[] VertexBoneWeights { get => vertexBoneWeights; set => vertexBoneWeights = value; }

        /// <summary> All frames in animation </summary>
        Frame[] frames;
        public Frame[] Frames { get => frames; set => frames = value; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="boneCount"> Number of bones </param>
        public SkinningAnimation(ObjLoader restPose, int boneCount, int frameCount)
        {
            this.RestPose = restPose;
            this.boneCount = boneCount;
            Frames = new Frame[frameCount];
            VertexBoneWeights = new Dictionary<int, double>[restPose.Vertices.Length];
            for (int i = 0; i < restPose.Vertices.Length; i++)
                VertexBoneWeights[i] = new Dictionary<int, double>();
            for (int i = 0; i < frameCount; i++)
                Frames[i] = new Frame(boneCount);
        }



    }
}
