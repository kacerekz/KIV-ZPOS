using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTkRenderer.Rendering.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Rendering.Materials
{
    /// <summary>
    /// Basic lit material
    /// </summary>
    public class BasicMaterial : Material
    {
        /// <summary>
        /// Shader to use
        /// </summary>
        public ShaderProgram shaderProgram;

        /// <summary>
        /// Sets skybox material parameters
        /// </summary>
        /// <param name="scene">Parent scene data</param>
        /// <param name="model">Model matrix</param>
        public override void Set(Scene scene, Matrix4 model)
        {
            shaderProgram.RegisterUniform("matrixProjection");
            shaderProgram.RegisterUniform("matrixView");
            shaderProgram.RegisterUniform("matrixModel");

            GL.UseProgram(shaderProgram.ID);
            GL.UniformMatrix4(shaderProgram.uniforms["matrixProjection"].Location, false, ref scene.camera.projection);
            GL.UniformMatrix4(shaderProgram.uniforms["matrixView"].Location, false, ref scene.camera.view);
            GL.UniformMatrix4(shaderProgram.uniforms["matrixModel"].Location, false, ref model);
        }

        /// <summary>
        /// Clears material
        /// </summary>
        public override void Clear()
        {
            GL.UseProgram(0);
        }
    }
}
