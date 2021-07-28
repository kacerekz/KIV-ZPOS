using OpenTkRenderer.Rendering.Meshes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshAnimation.Animation
{
    interface IAnimation
    {
        MeshLoader RestPose { get; set; }
        MeshLoader[] Frames { get; set; }

        bool LoadAnimation(string folder);
    }
}
