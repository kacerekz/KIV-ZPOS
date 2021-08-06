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
        public Vector<double>[] BoneTranslation { get => boneTranslation; set => boneTranslation = value; }

    }
}
