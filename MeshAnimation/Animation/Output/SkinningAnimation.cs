using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using OpenTkRenderer.Rendering.Meshes;

namespace MeshAnimation.Animation
{
    /// <summary>
    /// Output skinning animation
    /// </summary>
    public class SkinningAnimation
    {
        /// <summary> Number of bones </summary>
        int boneCount;


        /// <summary> Rest pose </summary>
        public ObjLoader RestPose { get; set; }
        
        /// <summary> All frames in animation </summary>
        public Frame[] Frames { get; set; }

        /// <summary> Bone-vertex weights - array of dictionaries for bones, key is vertex index, value is weight </summary>
        public Dictionary<int, double>[] VertexBoneWeights { get;  set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="restPose"> Animation rest pose </param>
        /// <param name="boneCount"> Number of bones </param>
        /// <param name="frameCount"> Number of frames </param>
        public SkinningAnimation(ObjLoader restPose, int boneCount, int frameCount)
        {
            this.RestPose = restPose;
            this.boneCount = boneCount;
            this.Frames = new Frame[frameCount];
            
            VertexBoneWeights = new Dictionary<int, double>[boneCount];
            
            for (int i = 0; i < VertexBoneWeights.Length; i++)
                VertexBoneWeights[i] = new Dictionary<int, double>();
            
            for (int i = 0; i < frameCount; i++)
                Frames[i] = new Frame(boneCount);
        }

        /// <summary>
        /// Empty constructor for serialization
        /// </summary>
        public SkinningAnimation()
        {
        }

    }
}
