using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    private bool timerIsRunning = false;

    [SerializeField]
    private TMPro.TextMeshProUGUI timerText;

    [SerializeField]
    private float timeRemaining = 30f;

    private bool hasGameEnded = false;

    [SerializeField]
    private GameObject gameOverMenuPrefab; // Reference to the Game Over menu prefab

    private void Start()
    {
        timerText.text = "Time: 30";
        timerIsRunning = true;
    }

    private void Update()
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
                hasGameEnded = true;
                timerText.text = "Game over!";
                ShowGameOverMenu();
            }
        }
    }

    private void ShowGameOverMenu()
    {
        // Instantiate the Game Over menu
        GameObject gameOverMenu = Instantiate(gameOverMenuPrefab);

        // Set up the button listener
        Button restartButton = gameOverMenu.GetComponentInChildren<Button>();
        restartButton.onClick.AddListener(RestartGame);
    }

    public void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        SceneManagerS1.Instance.LoadNextScene();    
    }

    public bool IsGameOver()
    {
        return hasGameEnded;
    }

    public void SetGameOver()
    {
        hasGameEnded = true;
        timerIsRunning = false;
        timerText.text = "Game over!";
        ShowGameOverMenu();
    }
}
