using System.Collections.Generic;

namespace Mert.SaveLoadSystem
{
    /// <summary>
    /// Interface for data services.
    /// </summary>
    public interface IDataService
    {
        void Save(GameData data, bool overwrite = true); // Saves the data.
        GameData Load(string name); // Loads the data.
        void Delete(string name); // Deletes the data.
        void DeleteAll(); // Deletes all data.
        IEnumerable<string> ListSaves(); // Lists all saves.
    }
}