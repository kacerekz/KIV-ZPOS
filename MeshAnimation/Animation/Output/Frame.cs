using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

namespace MeshAnimation.Animation
{
    public class Frame
    {
        private int boneCount;

        /// <summary> Bone rotations </summary>
        Matrix<double>[] boneRotation;
        public Matrix<double>[] BoneRotation { get => boneRotation; set => boneRotation = value; }

        /// <summary> Bone translation </summary>
        Vector<double>[] boneTranslation;
        public Vector<double>[] BoneTranslation { get => boneTranslation; set => boneTranslation = value; }

        public Frame(int boneCount)
        {
            this.boneCount = boneCount;
            BoneRotation = new Matrix<double>[boneCount];
            BoneTranslation = new Vector<double>[boneCount];
        }

    }
}
