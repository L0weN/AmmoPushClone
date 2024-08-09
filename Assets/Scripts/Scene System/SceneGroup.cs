using System;
using System.Collections.Generic;
using System.Linq;

namespace Mert.SceneManagement
{
    /// <summary>
    /// This class is used to store the scene group data.
    /// </summary>
    [Serializable]
    public class SceneGroup
    {
        public string GroupName = "New Scene Group";
        public List<SceneData> Scenes;

        public string FindSceneNameByType(SceneType sceneType) => Scenes.FirstOrDefault(scene => scene.SceneType == sceneType)?.Reference.Name;
    }
}