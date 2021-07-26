using OpenTK;
using OpenTkRenderer.Input;
using OpenTkRenderer.Rendering.Cameras;
using OpenTkRenderer.Rendering.Materials;
using OpenTkRenderer.Rendering.Meshes;
using System;
using System.Collections.Generic;

namespace OpenTkRenderer.Rendering.Scenes
{
    /// <summary>
    /// A scene to render
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// Maximum number of lights in a scene
        /// </summary>
        public static int MAX_LIGHTS = 1;

        /// <summary>
        /// Light shadow buffer
        /// </summary>
        public Framebuffer shadowBuffer;

        /// <summary>
        /// Active lights
        /// </summary>
        public List<Light> activeLights;

        /// <summary>
        /// Main camera in this scene
        /// </summary>
        public Camera camera;

        /// <summary>
        /// Objects in the scene and their properties
        /// </summary>
        public Dictionary<string, Mesh> meshes;
        public Dictionary<string, Material> materials;
        public Dictionary<string, ShaderProgram> shaders;
        public Dictionary<string, GameObject> gameObjects;
        
        /// <summary>
        /// Initializes a scene
        /// </summary>
        public Scene()
        {
            camera = new FlyCamera(new Vector3(0, 0, 0), new Vector3(0, 0, -1));
            InputManager.Attach(camera);
            MouseManager.Attach(camera);

            shadowBuffer = new Framebuffer(1024, 1024);
            shadowBuffer.GenerateLightAttachment(MAX_LIGHTS);
            shadowBuffer.CommitAttachments();

            activeLights = new List<Light>();
            meshes = new Dictionary<string, Mesh>();
            materials = new Dictionary<string, Material>();
            shaders = new Dictionary<string, ShaderProgram>();
            gameObjects = new Dictionary<string, GameObject>();
        }

        /// <summary>
        /// Returns all active objects in the scene
        /// </summary>
        /// <returns>Active objects in the scene</returns>
        public IEnumerable<GameObject> GetActiveGameObjects()
        {
            return gameObjects.Values;
        }
    }
}