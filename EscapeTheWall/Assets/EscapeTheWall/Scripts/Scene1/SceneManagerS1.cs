using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerS1 : Singleton<SceneManagerS1>
{

    private int currentSceneIndex = 0;
    [SerializeField]
    private int lastSceneIndex = 4;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadNextScene()
    {
        lastSceneIndex = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1;
        if (currentSceneIndex == lastSceneIndex)
        {
            // quit the game
            Application.Quit();
        }
        else
        {
            // Load the next scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
}
