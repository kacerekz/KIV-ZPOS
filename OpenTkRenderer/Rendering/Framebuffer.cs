using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;

namespace OpenTkRenderer.Rendering
{
    /// <summary>
    /// Framebuffer with optional color and depth attachments
    /// </summary>
    public class Framebuffer
    {
        /// <summary>
        /// Framebuffer ID
        /// </summary>
        public int ID;

        /// <summary>
        ///  Framebuffer width
        /// </summary>
        public int width;

        /// <summary>
        ///  Framebuffer height
        /// </summary>
        public int height;

        /// <summary>
        /// Color buffer ID
        /// </summary>
        public int colorBuffer = -1;

        /// <summary>
        /// Depth buffer ID
        /// </summary>
        public int depthBuffer = -1;

        /// <summary>
        /// Framebuffer status
        /// </summary>
        public FramebufferStatus status;

        /// <summary>
        /// Initializes the Framebuffer
        /// </summary>
        /// <param name="width">Buffer width</param>
        /// <param name="height">Buffer height</param>
        public Framebuffer(int width, int height)
        {
            this.width = width;
            this.height = height;

            ID = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, ID);
        }

        /// <summary>
        /// Generate a color attachment
        /// </summary>
        public void GenerateColorAttachment()
        {
            colorBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, colorBuffer);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16f, width, height, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, colorBuffer, 0);
        }

        /// <summary>
        /// Generate a depth attachment
        /// </summary>
        public void GenerateLightAttachment(int lightCount)
        {
            depthBuffer = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2DArray, depthBuffer);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureCompareMode, (int)TextureCompareMode.CompareRefToTexture);

            GL.TexImage3D(TextureTarget.Texture2DArray, 0, PixelInternalFormat.DepthComponent32f,
                width, height, lightCount, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.FramebufferTextureLayer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, depthBuffer, 0, 0);
        }

        /// <summary>
        /// Call after creating all required attachments
        /// </summary>
        public void CommitAttachments()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            status = GL.CheckNamedFramebufferStatus(ID, FramebufferTarget.Framebuffer);
        }

    }
}
