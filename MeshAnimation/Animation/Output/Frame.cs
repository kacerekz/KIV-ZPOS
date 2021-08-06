using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;

namespace MeshAnimation.Animation
{
    class Frame
    {
        /// <summary> Bone rotations </summary>
        Matrix<double>[] boneRotation;
        public Matrix<double>[] BoneRotation { get => boneRotation; set => boneRotation = value; }
        /// <summary> Bone translation </summary>
        Vector<double>[] boneTranslation;
        private int boneCount;

        public Frame(int boneCount)
        {
            this.boneCount = boneCount;
            BoneRotation = new Matrix<double>[boneCount];
            BoneTranslation = new Vector<double>[boneCount];
        }

        public Vector<double>[] BoneTranslation { get => boneTranslation; set => boneTranslation = value; }

    }
}
