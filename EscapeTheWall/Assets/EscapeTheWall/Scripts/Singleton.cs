using UnityEngine;

/// <summary>
/// A generic singleton base class for MonoBehaviour components.
/// Ensures that only one instance of a given type exists in the scene.
/// </summary>
/// <typeparam name="T">The type of the singleton class.</typeparam>
public class Singleton<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// The static instance of the singleton class.
    /// </summary>
    public static T Instance { get; private set; }

    /// <summary>
    /// Ensures that only one instance of the singleton exists.
    /// Destroys any duplicate instances.
    /// </summary>
    private void Awake()
    {
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            // Destroy the duplicate instance
            Destroy(this.gameObject);
        }
        else
        {
            // Assign the instance if it doesn't already exist
            Instance = this as T;
        }
    }
}
