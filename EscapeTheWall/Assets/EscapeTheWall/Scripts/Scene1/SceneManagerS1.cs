using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerS1 : Singleton<SceneManagerS1>
{
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadNextScene()
    {
        // Load the next scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
}
