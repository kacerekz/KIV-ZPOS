using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshAnimation
{
    /// <summary>
    /// Program configuration
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Mode to launch in
        /// </summary>
        public Mode mode = Mode.Render;

        /// <summary>
        /// Path to file / folder
        /// </summary>
        public string path = @"C:\Users\Daemon\Desktop\res\with_weighted_centroids\samba_clustering\samba";

        /// <summary>
        /// Path to output files - without suffix
        /// </summary>
        public string outName = @".\jump";

        /// <summary>
        /// Number of ssdr iteration
        /// </summary>
        public int ssdrIterations = 0;

        /// <summary>
        /// Number of bones
        /// </summary>
        public int boneCount = 15;

        /// <summary>
        /// Number of significant bones
        /// </summary>
        public int significantBoneCount = 4;

        /// <summary>
        /// Scale model by
        /// </summary>
        public float scaleModel = 1;

        /// <summary>
        /// Tolerance of bone influence for re-initialization
        /// Any bone with smaller influence will be re-initialized
        /// </summary>
        public int toleranceForReinit = 3;

        /// <summary>
        /// Number of vertices to assign to an re-initialized bone
        /// </summary>
        public int neighbourCount = 20;

        /// <summary>
        /// Use genetic algorithm for weight update, other option is Non-negative least squares
        /// </summary>
        public bool geneticAlgorithm = false;

        /// <summary>
        /// Number of individuals in one population
        /// </summary>
        public int generationSize = 100;

        /// <summary>
        /// Number of max iterations for the genetic algorithm
        /// </summary>
        public int geneticIterations = 100;

        /// <summary>
        /// Window update frequency
        /// </summary>
        public int updatesPerSecond = 100;

        /// <summary>
        /// Window FPS cap
        /// </summary>
        public int framesPerSecond = 100;

        /// <summary>
        /// Window width
        /// /// </summary>
        public int windowWidth = 800;

        /// <summary>
        /// Window height
        /// </summary>
        public int windowHeight = 600;


    }
}
