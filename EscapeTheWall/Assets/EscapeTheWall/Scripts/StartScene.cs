using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    // Start button
    [SerializeField] 
    private GameObject startButton;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManagerETW.Instance.LoadNextScene();
    }
}
