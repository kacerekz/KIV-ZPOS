using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace OpenTkRenderer.Rendering
{
    /// <summary>
    /// Represents a complete shader program, composed of individual shaders
    /// </summary>
    public class ShaderProgram
    {
        /// <summary>
        /// OpenGL object ID
        /// </summary>
        public int ID;

        /// <summary>
        /// Linking status
        /// </summary>
        public bool linked;

        /// <summary>
        /// Linking log
        /// </summary>
        public string log;

        /// <summary>
        /// Shader uniforms
        /// </summary>
        public Dictionary<string, UniformInfo> uniforms;

        /// <summary>
        /// Shader attributes
        /// </summary>
        public Dictionary<string, AttribInfo> attribs;

        /// <summary>
        /// Creates a complete shader program
        /// </summary>
        /// <param name="shaders">Shaders to be attached</param>
        public ShaderProgram(params Shader[] shaders)
        {
            ID = GL.CreateProgram();
            foreach (var shader in shaders)
            {
                GL.AttachShader(ID, shader.ID);
                log += shader.type + shader.log;
            }
            GL.LinkProgram(ID);

            GL.GetProgram(ID, GetProgramParameterName.LinkStatus, out var code);
            linked = (code != (int)OpenTK.Graphics.OpenGL.Boolean.False);
            log += GL.GetProgramInfoLog(ID);

            if (!linked)
                Console.WriteLine(log);

            foreach (var shader in shaders)
            {
                GL.DetachShader(ID, shader.ID);
            }

            uniforms = new Dictionary<string, UniformInfo>();

            if (linked)
            {
                GL.GetProgram(ID, GetProgramParameterName.ActiveUniforms, out var count);
                for (int i = 0; i < count; i++)
                {
                    string name = GL.GetActiveUniform(ID, i, out var size, out var type);
                    int location = GL.GetUniformLocation(ID, name);
                    uniforms.Add(name, new UniformInfo() { Name = name, Location = location, Size = size, Type = type });
                }

                attribs = new Dictionary<string, AttribInfo>();
                GL.GetProgram(ID, GetProgramParameterName.ActiveAttributes, out count);
                for (int i = 0; i < count; i++)
                {
                    string name = GL.GetActiveAttrib(ID, i, out var size, out var type);
                    int location = GL.GetAttribLocation(ID, name);
                    attribs.Add(name, new AttribInfo() { Name = name, Location = location, Size = size, Type = type });
                }
            }
        }

        /// <summary>
        /// Registers an unused uniform
        /// </summary>
        /// <param name="key">Uniform name</param>
        public void RegisterUniform(string key)
        {
            if (!uniforms.ContainsKey(key))
            {
                uniforms.Add(key, new UniformInfo() { Location = -1 });
            }
        }

        /// <summary>
        /// Sets the shader program to be deleted
        /// </summary>
        public void Delete()
        {
            GL.GetProgram(ID, GetProgramParameterName.DeleteStatus, out var code);
            if (code == (int)All.False)
                GL.DeleteProgram(ID);
        }

    }
}
