using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private bool gameEnded = false;
    private bool isPaused = false;
    private int levelIndex = 0;

    [SerializeField]
    private TMPro.TextMeshProUGUI timerText;

    [SerializeField]
    private float timeRemaining = 30f; 
    private bool timerIsRunning = false;

    void Start()
    {
        timerText.text = "Time: 30";
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timerText.text = "Time: " + timeRemaining.ToString("F0");
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    public void LoadNextLevel()
    {
        levelIndex++;

        if (levelIndex < 2)
        {
            SceneManager.LoadScene(levelIndex);
        }
        else
        {
            Application.Quit();
        }
    }
}
