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

            for (int i = 0; i < scene.activeLights.Count; i++)
            {
                // OpenTK matrices are transposed ? -> multiply in opposite order ?
                Light l = scene.activeLights[i];
                Matrix4 lightMatrix = l.BuildViewMatrix() * l.BuildProjectionMatrix();

                shaderProgram.RegisterUniform($"light[{i}].position");
                shaderProgram.RegisterUniform($"light[{i}].direction");

                shaderProgram.RegisterUniform($"light[{i}].ambient");
                shaderProgram.RegisterUniform($"light[{i}].diffuse");
                shaderProgram.RegisterUniform($"light[{i}].specular");

                shaderProgram.RegisterUniform($"light[{i}].linear");
                shaderProgram.RegisterUniform($"light[{i}].quadratic");

                shaderProgram.RegisterUniform($"light[{i}].cutoff");
                shaderProgram.RegisterUniform($"light[{i}].outerCutoff");

                shaderProgram.RegisterUniform($"light[{i}].matrix");

                GL.Uniform3(shaderProgram.uniforms[$"light[{i}].position"].Location, l.position);
                GL.Uniform3(shaderProgram.uniforms[$"light[{i}].direction"].Location, l.direction);

                GL.Uniform3(shaderProgram.uniforms[$"light[{i}].ambient"].Location, l.ambient);
                GL.Uniform3(shaderProgram.uniforms[$"light[{i}].diffuse"].Location, l.diffuse);
                GL.Uniform3(shaderProgram.uniforms[$"light[{i}].specular"].Location, l.specular);

                GL.Uniform1(shaderProgram.uniforms[$"light[{i}].linear"].Location, l.linear);
                GL.Uniform1(shaderProgram.uniforms[$"light[{i}].quadratic"].Location, l.quadratic);

                GL.Uniform1(shaderProgram.uniforms[$"light[{i}].cutoff"].Location, l.cutoff);
                GL.Uniform1(shaderProgram.uniforms[$"light[{i}].outerCutoff"].Location, l.outerCutoff);

                GL.UniformMatrix4(shaderProgram.uniforms[$"light[{i}].matrix"].Location, false, ref lightMatrix);
            }

            shaderProgram.RegisterUniform("shadowTex");

            GL.Uniform1(shaderProgram.uniforms["shadowTex"].Location, 0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2DArray, scene.shadowBuffer.depthBuffer);
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
