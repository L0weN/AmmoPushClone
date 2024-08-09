using Eflatun.SceneReference;
using System;

namespace Mert.SceneManagement
{
    /// <summary>
    /// This class is used to store the scene data.
    /// </summary>
    [Serializable]
    public class SceneData
    {
        public SceneReference Reference;
        public string Name => Reference.Name;
        public SceneType SceneType;
    }
}