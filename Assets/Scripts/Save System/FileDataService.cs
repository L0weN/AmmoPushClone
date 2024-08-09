using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mert.SaveLoadSystem
{
    /// <summary>
    /// This class is responsible for managing a file data service.
    /// </summary>
    public class FileDataService : IDataService
    {
        ISerializer serializer; // Serializer to serialize and deserialize data.
        string dataPath; // Path to the data.
        string fileExtension; // Extension of the file.

        /// <summary>
        /// This method initializes the file data service.
        /// </summary>
        /// <param name="serializer"></param>
        public FileDataService(ISerializer serializer)
        {
            dataPath = Application.persistentDataPath;
            fileExtension = "json";
            this.serializer = serializer;
        }

        /// <summary>
        /// This method returns the path to the file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        string GetPathToFile(string fileName)
        {
            return Path.Combine(dataPath, string.Concat(fileName, ".", fileExtension));
        }

        /// <summary>
        /// This method saves the data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="overwrite"></param>
        /// <exception cref="IOException"></exception>
        public void Save(GameData data, bool overwrite = true)
        {
            string fileLocation = GetPathToFile(data.Name);

            if (!overwrite && File.Exists(fileLocation))
            {
                throw new IOException($"The file '{data.Name}.{fileExtension}' already exists and cannot be overwritten.");
            }

            File.WriteAllText(fileLocation, serializer.Serialize(data));
        }

        /// <summary>
        /// This method loads the data.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameData Load(string name)
        {
            string fileLocation = GetPathToFile(name);

            if (!File.Exists(fileLocation))
            {
                GameData gameData = new GameData { Name = name };
                Save(gameData);
                //throw new ArgumentException($"No persisted GameData with name '{name}'");
            }

            return serializer.Deserialize<GameData>(File.ReadAllText(fileLocation));
        }

        /// <summary>
        /// This method deletes the data.
        /// </summary>
        /// <param name="name"></param>
        public void Delete(string name)
        {
            string fileLocation = GetPathToFile(name);

            if (File.Exists(fileLocation))
            {
                File.Delete(fileLocation);
            }
        }

        /// <summary>
        /// This method deletes all data.
        /// </summary>
        public void DeleteAll()
        {
            foreach (string filePath in Directory.GetFiles(dataPath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Lists all saves.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ListSaves()
        {
            foreach (string path in Directory.EnumerateFiles(dataPath))
            {
                if (Path.GetExtension(path) == fileExtension)
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }
    }
}