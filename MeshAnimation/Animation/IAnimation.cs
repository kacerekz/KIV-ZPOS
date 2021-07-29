using OpenTkRenderer.Rendering.Meshes;

namespace MeshAnimation.Animation
{
    /// <summary>
    /// Input animation interface
    /// </summary>
    interface IAnimation
    {
        /// <summary> Rest pose </summary>
        MeshLoader RestPose { get; set; }
        /// <summary> Animation frames </summary>
        MeshLoader[] Frames { get; set; }

        /// <summary>
        /// Load animation from folder
        /// </summary>
        /// <param name="folder"> Path to folder </param>
        /// <returns> True if successful, false if not </returns>
        bool LoadAnimation(string folder);
    }
}
