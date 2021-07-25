using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MeshAnimation.Util
{
    /// <summary>
    /// Serialization utility
    /// </summary>
    public class Serialization
    {
        /// <summary>
        /// Serialize to string
        /// </summary>
        /// <typeparam name="T">Type of instance to serialize</typeparam>
        /// <param name="data">Instance to serialize</param>
        /// <returns>Serialized instance</returns>
        public static string Serialize<T>(T data)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, data);
                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Serialize to file
        /// </summary>
        /// <typeparam name="T">Type of instance to serialize</typeparam>
        /// <param name="data">Instance to serialize</param>
        /// <param name="path">File to create</param>
        public static void Serialize<T>(T data, string path)
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            FileStream stream = new FileStream(path, FileMode.Create);
            xml.Serialize(stream, data);
            stream.Close();
        }

        /// <summary>
        /// Deserialization.
        /// If isString is true, data is expected to be the instance to deserialize, as a string.
        /// Otherwise, data is expected to be the path to the file in which the instance is stored.
        /// By default, a path is expected.
        /// </summary>
        /// <typeparam name="T">Type of instance to deserialize</typeparam>
        /// <param name="data">Instance or path to deserialize</param>
        /// <param name="isString">True if data is the instance, false if it's the file path</param>
        /// <returns>Deserialized instance</returns>
        public static T Deserialize<T>(string data, bool isString = false)
        {
            if (isString)
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                using (StringReader stream = new StringReader(data))
                {
                    return (T)xml.Deserialize(stream);
                }
            }
            else
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                FileStream stream = new FileStream(data, FileMode.Open);
                return (T)xml.Deserialize(stream);
            }
        }
    }
}
