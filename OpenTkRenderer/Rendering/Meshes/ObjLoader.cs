using OpenTkRenderer.Structs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkRenderer.Rendering.Meshes
{
    /// <summary>
    /// OBJ file loader
    /// </summary>
    public class ObjLoader : MeshLoader
    {
        // TODO beef this up later to read the TVM as well

        /// <summary>
        /// Load a simple OBJ from file
        /// </summary>
        /// <param name="filename">Path to input file</param>
        public override void Load(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                List<Vec3f> vertices = new List<Vec3f>();
                List<Vec3f> normals = new List<Vec3f>();
                List<Vec3i> indices = new List<Vec3i>();

                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var split = line.Split(' ');

                    switch (split[0])
                    {
                        case "v":
                            var v = new Vec3f
                            {
                                x = float.Parse(split[1]),
                                y = float.Parse(split[2]),
                                z = float.Parse(split[3])
                            };
                            vertices.Add(v);
                            break;

                        case "vn":
                            var vn = new Vec3f
                            {
                                x = float.Parse(split[1]),
                                y = float.Parse(split[2]),
                                z = float.Parse(split[3])
                            };
                            normals.Add(vn);
                            break;

                        case "f":
                            if (line.Contains("/"))
                            {
                                var splitx = split[1].Split('/');
                                var splity = split[1].Split('/');
                                var splitz = split[1].Split('/');

                                var f = new Vec3i
                                {
                                    v1 = uint.Parse(splitx[0]) - 1,
                                    v2 = uint.Parse(splity[0]) - 1,
                                    v3 = uint.Parse(splitz[0]) - 1
                                };
                                indices.Add(f);
                            }
                            else
                            {
                                var f = new Vec3i
                                {
                                    v1 = uint.Parse(split[1]) - 1,
                                    v2 = uint.Parse(split[2]) - 1,
                                    v3 = uint.Parse(split[3]) - 1
                                };
                                indices.Add(f);
                            }
                            
                            break;
                        
                        default:
                            break;
                    }
                } // end while !EOF

                Vertices = vertices.ToArray();
                Normals = normals.ToArray();
                Indices = indices.ToArray();

            } // end using
        } // end Load()

    }
}
