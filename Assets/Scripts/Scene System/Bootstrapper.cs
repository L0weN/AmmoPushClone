using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is used to load the first scene of the game.
/// </summary>
public class Bootstrapper : SingleBehaviour<Bootstrapper>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        Debug.Log("Bootstrapper...");
        SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
    }
}
