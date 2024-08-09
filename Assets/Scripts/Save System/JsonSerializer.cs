using UnityEngine;

namespace Mert.SaveLoadSystem
{
    /// <summary>
    /// This class is responsible for serializing and deserializing objects to and from JSON format.
    /// </summary>
    public class JsonSerializer : ISerializer
    {
        /// <summary>
        /// This method serializes the object to JSON format.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize<T>(T obj)
        {
            return JsonUtility.ToJson(obj, true);
        }

        /// <summary>
        /// This method deserializes the JSON string to the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T Deserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}