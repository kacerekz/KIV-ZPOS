using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace OpenTkRenderer.Rendering
{
    /// <summary>
    /// Representing and manipulating shaders
    /// </summary>
    public class Shader
    {
        /// <summary>
        /// Shader object ID
        /// </summary>
        public int ID;

        /// <summary>
        /// Compilation status
        /// </summary>
        public bool compiled;

        /// <summary>
        /// Compilation log
        /// </summary>
        public string log;

        /// <summary>
        /// Type of shader
        /// </summary>
        public ShaderType type;

        /// <summary>
        /// Creates a shader of given type from source code given as a string
        /// </summary>
        /// <param name="type">Type of shader to create</param>
        /// <param name="source">Shader source code</param>
        public Shader(ShaderType type, string source)
        {
            this.type = type;
            ID = GL.CreateShader(type);
            GL.ShaderSource(ID, source);

            GL.CompileShader(ID);

            GL.GetShader(ID, ShaderParameter.CompileStatus, out var code);
            compiled = (code != (int)OpenTK.Graphics.OpenGL.Boolean.False);
            log = source + "\n" + GL.GetShaderInfoLog(ID);
        }

        /// <summary>
        /// Sets shader to be deleted
        /// </summary>
        public void Delete()
        {
            GL.GetShader(ID, ShaderParameter.DeleteStatus, out var code);
            if (code == (int)All.False)
                GL.DeleteShader(ID);
        }
    }

    /// <summary>
    /// A vertex shader
    /// </summary>
    public class VertexShader : Shader
    {
        public VertexShader(string source) : base(ShaderType.VertexShader, source) { }
    }

    /// <summary>
    /// A fragment shader
    /// </summary>
    public class FragmentShader : Shader
    {
        public FragmentShader(string source) : base(ShaderType.FragmentShader, source) { }
    }

    /// <summary>
    /// A geometry shader
    /// </summary>
    public class GeometryShader : Shader
    {
        public GeometryShader(string source) : base(ShaderType.GeometryShader, source) { }
    }
}
