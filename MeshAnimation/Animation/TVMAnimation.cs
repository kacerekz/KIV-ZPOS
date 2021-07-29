using MeshAnimation.Util;
using OpenTkRenderer.Rendering.Meshes;
using System;

namespace MeshAnimation.Animation
{
    /// <summary>
    /// Time varying mesh animation
    /// </summary>
    class TVMAnimation : IAnimation
    {
        /// <summary> Rest pose </summary>
        MeshLoader restPose;
        public MeshLoader RestPose { get => restPose; set => restPose = value; }

        /// <summary> Animation frames </summary>
        MeshLoader[] frames;
        public MeshLoader[] Frames { get => frames; set => frames = value; }

        /// <summary>
        /// Load animation frames from folder, set up rest pose
        /// </summary>
        /// <param name="folder"> Path to folder </param>
        /// <returns> True if successful, false if not </returns>
        public bool LoadAnimation(string folder)
        {
            Console.WriteLine("Loading animation " + folder);

            // get all obj files from folder
            string[] files = FileProcessing.GetObjFromDir(folder);

            if (files.Length <= 0)
                return false;

            // load all obj files to frames
            frames = new MeshLoader[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                frames[i] = new ObjLoader();
                frames[i].Load(files[i]);
            }

            // TODO rest pose
            Console.WriteLine("Missing rest pose");
            throw new NotImplementedException();

            Console.WriteLine("Finished loading animation " + folder);
            return true;
        }
    }
}
