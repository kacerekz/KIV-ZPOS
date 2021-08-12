using MathNet.Numerics.LinearAlgebra;
using MeshAnimation.Animation;
using OpenTK;
using OpenTkRenderer.Input;
using OpenTkRenderer.Rendering;
using OpenTkRenderer.Rendering.Materials;
using OpenTkRenderer.Rendering.Meshes;
using OpenTkRenderer.Rendering.Scenes;
using OpenTkRenderer.Structs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshAnimation.Scenes
{
    public class AnimationScene : Scene, IInputObserver
    {
        public static GameObject[] gameObjects;
        public static int activeIndex;

        public static void CreateScene(SkinningAnimation animation, float scale = 1)
        {
            // Create light
            var light = new Light(
                // position
                new Vector3(5.0f, 5.0f, -5.0f),
                // direction
                new Vector3(-1.0f, -1.0f, 1.0f));

            light.SetParameters(
                // ambient
                new Vector3(1, 1, 1),
                // diffuse
                new Vector3(0.8f, 0.8f, 0.8f),
                // specular
                new Vector3(0.2f, 0.2f, 0.2f));

            light.SetAttenuation(0.09f, 0.032f);
            light.SetCutoff(20f, 30f);

            SceneManager.ActiveScene.activeLights.Add(light);

            MeshLoader[] loaders = new MeshLoader[animation.Frames.Length];
            gameObjects = new GameObject[animation.Frames.Length];

            for (int i = 0; i < loaders.Length; i++)
            {
                loaders[i] = Transform(animation.RestPose, animation.VertexBoneWeights, animation.Frames[i]);
                
                // TODO here different shader
                gameObjects[i] = LoadObject(loaders[i], "Shaders/colours.vert", "Shaders/colours.frag");
                gameObjects[i].disabled = i != 0;
                gameObjects[i].transform *= Matrix4.CreateScale(scale);

                SceneManager.ActiveScene.gameObjects.Add($"{i}", gameObjects[i]);
            }


            // Add ground
            GameObject plane = LoadObject("Data/plane.obj");
            plane.transform *= Matrix4.CreateScale(10.0f);
            plane.transform *= Matrix4.CreateTranslation(0.0f, 0.0f, -5.0f);
            SceneManager.ActiveScene.gameObjects.Add("plane", plane);


            // Add to input map
            InputManager.mapping["Next"] = OpenTK.Input.Key.Right;
            InputManager.mapping["Previous"] = OpenTK.Input.Key.Left;
            InputManager.Attach(new AnimationScene());
        }

        private static MeshLoader Transform(MeshLoader restPose, ConcurrentDictionary<int, double>[] vertexBoneWeights, Frame frame)
        {
            Vec3f[] vertices = new Vec3f[restPose.Vertices.Length];
            int boneCount = vertexBoneWeights.Length;
            var V = Vector<double>.Build;

            for (int b = 0; b < boneCount; b++)
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    if (vertexBoneWeights[b].ContainsKey(i))
                    {
                        var ov = restPose.Vertices[i];
                        var v = V.Dense(new double[] { ov.x, ov.y, ov.z });

                        v = vertexBoneWeights[b][i] * frame.BoneRotation[b] * v + vertexBoneWeights[b][i] * frame.BoneTranslation[b];

                        vertices[i].x += (float)v[0];
                        vertices[i].y += (float)v[1];
                        vertices[i].z += (float)v[2];
                    }
                }
            }

            MeshLoader mesh = new MeshLoader();
            mesh.Vertices = vertices;
            mesh.Normals= restPose.Normals;
            mesh.Colors = restPose.Colors;
            mesh.TexCoords = restPose.TexCoords;
            mesh.Indices = restPose.Indices;

            return mesh;
        }

        public void Update(IDictionary<string, bool> state)
        {
            if (state.ContainsKey("Next") && state["Next"])
            {
                gameObjects[activeIndex].disabled = true;
                activeIndex++;
                if (activeIndex >= gameObjects.Length)
                    activeIndex = 0;
                gameObjects[activeIndex].disabled = false;
                Console.WriteLine($"Activated {activeIndex}");
            }

            if (state.ContainsKey("Previous") && state["Previous"])
            {
                gameObjects[activeIndex].disabled = true;
                activeIndex--;
                if (activeIndex < 0)
                    activeIndex = gameObjects.Length - 1;
                gameObjects[activeIndex].disabled = false;
                Console.WriteLine($"Activated {activeIndex}");
            }
        }
    }
}
