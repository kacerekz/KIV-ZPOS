using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;
using OpenTkRenderer.Structs;

namespace OpenTkRenderer.Rendering.Meshes
{
    /// <summary>
    /// Represents an mesh loaded from a file
    /// </summary>
    public class BasicMesh : Mesh
    {
        /// <summary>
        /// VAO ID
        /// </summary>
        int vao;

        /// <summary>
        /// Buffer IDs
        /// </summary>
        int vb, ib;

        /// <summary>
        /// Index count
        /// </summary>
        int indexCount;


        /// <summary>
        /// Load the mesh
        /// </summary>
        /// <param name="loader">Mesh loader with the mesh loaded in</param>
        public BasicMesh(MeshLoader loader)
        {
            Vec3f[] vertices = loader.Vertices;
            Vec3f[] normals = loader.Normals;
            Vec3f[] colors = loader.Colors;
            Vec2f[] texCoords = loader.TexCoords;
            Vec3i[] indices = loader.Indices;

            indexCount = indices.Length;

            int bpv = 0;
            if (vertices != null) bpv += 3 * sizeof(float);
            if (normals != null) bpv += 3 * sizeof(float);
            if (colors != null) bpv += 3 * sizeof(float);
            if (texCoords != null) bpv += 2 * sizeof(float);

            // Generating vertex array object
            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            // Generating and attaching the vertex array buffer
            vb = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vb);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * bpv, IntPtr.Zero, BufferUsageHint.StaticDraw);

            // Loading in the data to subbuffers, keep track of the offset
            //  + Connecting data in the buffer to attribute arrays that will be exposed in shader code
            // 0 - positions
            // 2 - normals
            // 3 - colors
            // 8 - texture coordinates [0]
            int offset = 0;

            // Position data
            GL.BufferSubData(BufferTarget.ArrayBuffer,
                (IntPtr)offset,
                vertices.Length * 3 * sizeof(float),
                vertices);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false,
                0, (IntPtr)offset);

            offset += vertices.Length * 3 * sizeof(float);

            // Normals
            if (normals != null)
            {
                GL.BufferSubData(BufferTarget.ArrayBuffer,
                (IntPtr)offset,
                normals.Length * 3 * sizeof(float),
                normals);

                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false,
                    0, (IntPtr)(vertices.Length * offset));

                offset += normals.Length * 3 * sizeof(float);
            }

            // Colors
            if (colors != null)
            {
                GL.BufferSubData(BufferTarget.ArrayBuffer,
                (IntPtr)offset,
                colors.Length * 3 * sizeof(float),
                colors);

                GL.EnableVertexAttribArray(3);
                GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false,
                    0, (IntPtr)(vertices.Length * offset));

                offset += colors.Length * 3 * sizeof(float);
            }

            // Texture coordinates
            if (texCoords != null)
            {
                GL.BufferSubData(BufferTarget.ArrayBuffer,
                (IntPtr)offset,
                texCoords.Length * 2 * sizeof(float),
                texCoords);

                GL.EnableVertexAttribArray(8);
                GL.VertexAttribPointer(8, 2, VertexAttribPointerType.Float, false,
                    0, (IntPtr)(vertices.Length * offset));

                //offset += texCoords.Length * 2 * sizeof(float);
            }

            // Create and fill the index buffer
            ib = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ib);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indexCount * 3 * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            // Will be deleted when no longer in use
            GL.DeleteBuffer(vb);
            GL.DeleteBuffer(ib);
        }

        /// <summary>
        /// Render the mesh
        /// </summary>
        public override void Render()
        {
            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, indexCount * 3, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        /// <summary>
        /// Destroys mesh
        /// </summary>
        public override void Destroy()
        {
            GL.DeleteVertexArray(vao);
        }

    }
}
