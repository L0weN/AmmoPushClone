using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mert.SaveLoadSystem
{
    /// <summary>
    /// This class is responsible for managing the save and load operations.
    /// </summary>
    [Serializable]
    public class GameData
    {
        public string Name; // Name of the game
        public PlayerData playerData; // Player data
    }

    /// <summary>
    /// This interface is used to mark the classes that can be saved.
    /// </summary>
    public interface ISaveable
    {
        SerializableGuid Id { get; set; }
    }

    /// <summary>
    /// Bind interface is used to bind the data to the entity.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IBind<TData> where TData : ISaveable
    {
        SerializableGuid Id { get; }
        void Bind(TData data);
    }

    /// <summary>
    /// This class is responsible for managing the save and load operations.
    /// </summary>
    public class SaveManager : SingleBehaviour<SaveManager>
    {
        [HideInInspector] public GameData gameData; // Game data
        IDataService dataService; // Data service

        protected override void Awake()
        {
            base.Awake();
            dataService = new FileDataService(new JsonSerializer());
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            Bind<Player, PlayerData>(gameData.playerData);
        }

        /// <summary>
        /// Bind the data to the entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="data"></param>
        void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
            if (entity != null)
            {
                if (data == null)
                {
                    data = new TData { Id = entity.Id };
                }
                entity.Bind(data);
            }
        }

        /// <summary>
        /// Bind the data to the entities.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="datas"></param>
        void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entities = FindObjectsByType<T>(FindObjectsSortMode.None);

            foreach (var entity in entities)
            {
                var data = datas.FirstOrDefault(d => d.Id == entity.Id);
                if (data == null)
                {
                    data = new TData { Id = entity.Id };
                    datas.Add(data);
                }
                entity.Bind(data);
            }
        }

        public GameData GetGameData() => gameData; // Get the game data
        public void SetGameData(GameData gameData) => this.gameData = gameData; // Set the game data

        /// <summary>
        /// Create a new game.
        /// </summary>
        public void NewGame() => gameData = new GameData
        {
            Name = "New Game",
        };

        public void SaveGame() => dataService.Save(gameData); // Save the game

        public void LoadGame(string gameName) => gameData = dataService.Load(gameName); // Load the game

        public void ReloadGame() => LoadGame(gameData.Name); // Reload the game

        public void DeleteGame(string gameName) => dataService.Delete(gameName); // Delete the game
    }
}