using System;

namespace Mert.SceneManagement
{
    /// <summary>
    /// This class is used to report the loading progress of the scene.
    /// </summary>
    public class LoadingProgress : IProgress<float>
    {
        public event Action<float> Progressed;

        const float ratio = 1f;

        public void Report(float value)
        {
            Progressed?.Invoke(value / ratio);
        }
    }
}