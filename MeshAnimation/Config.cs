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
        public string path = @"D:\moje\school\04\zpos\ZPOS data\constant connectivity\jump";

        /// <summary>
        /// Path to output files - without suffix
        /// </summary>
        public string outName = @".\jump";

        /// <summary>
        /// Number of ssdr iteration
        /// </summary>
        public int ssdrIterations = 10;

        /// <summary>
        /// Number of bones
        /// </summary>
        public int boneCount = 8;

        /// <summary>
        /// Number of significant bones
        /// </summary>
        public int significantBoneCount = 4;

        /// <summary>
        /// Scale model by
        /// </summary>
        public float scaleModel = 0.03f;

        /// <summary>
        /// Tolerance of bone influence for re-initialization
        /// Any bone with smaller influence will be re-initialized
        /// </summary>
        public int toleranceForReinit = 3;

        /// <summary>
        /// Number of vertices to assign to an re-initialized bone
        /// </summary>
        public int neighbourCount = 200;

        /// <summary>
        /// Use genetic algorithm for weight update, other option is Non-negative least squares
        /// </summary>
        public bool geneticAlgorithm = true;

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
        /// Window update frequency
        /// </summary>
        public int windowWidth = 800;

        /// <summary>
        /// Window FPS cap
        /// </summary>
        public int windowHeight = 600;


    }
}
