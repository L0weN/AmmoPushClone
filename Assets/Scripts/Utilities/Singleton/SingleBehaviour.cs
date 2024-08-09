using UnityEngine;

/// <summary>
/// When inheriting from this class, it will make sure that there is only one instance of the class in the scene.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingleBehaviour<T> : MonoBehaviour where T : Component
{
    public bool AutoUnparentOnAwake = true;

    protected static T instance;

    public static bool HasInstance => instance != null;
    public static T TryGetInstance() => HasInstance ? instance : null;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
                if (instance == null)
                {
                    var go = new GameObject(typeof(T).Name + " Auto-Generated");
                    instance = go.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        InitializeSingleton();
    }

    protected virtual void InitializeSingleton()
    {
        if (!Application.isPlaying) return;

        if (AutoUnparentOnAwake)
        {
            transform.SetParent(null);
        }

        if (instance == null)
        {
            instance = this as T;
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
