using UnityEngine;

/// A class which has only one instance in a scene.
/// You have to add it as a component to any GameObject. It will be initialised in Awake() method.
/// The singleton instance is destroyed when the scene (in which it was instantiated) is unloaded.
public class SceneSingletonBehaviour<T> : MonoBehaviour where T : SceneSingletonBehaviour<T>
{
    private static T instance;

    public static T Instance {
        get { return instance; }
    }


    /// Base awake method that sets the singleton's unique instance.
    protected virtual void Awake()
    {
        if (instance != null) {
            Debug.LogErrorFormat("Trying to instantiate a second instance of singleton class: {0}", GetType().Name);
        } else {
            instance = (T) this;
        }
    }


    /// When the scene is unloaded, the singleton instance is deallocated as well
    protected virtual void OnDestroy()
    {
        if (instance == this) {
            instance = null;
        }
    }
}