using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private int levelIndex = 0;
    private bool timerIsRunning = false;

    [SerializeField]
    private TMPro.TextMeshProUGUI timerText;

    [SerializeField]
    private float timeRemaining = 30f;

    public bool HasGameEnded = false;

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
                HasGameEnded = true;
                timerText.text = "Game over!";
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
