namespace Mert.SaveLoadSystem
{
    /// <summary>
    /// Interface for serializers.
    /// </summary>
    public interface ISerializer
    {
        string Serialize<T>(T obj); // Serializes the object.
        T Deserialize<T>(string json); // Deserializes the object.
    }
}