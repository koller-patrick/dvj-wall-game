using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerETW : Singleton<SceneManagerETW>
{

    private int currentSceneIndex = 0;
    [SerializeField]
    private int lastSceneIndex = 4;

    
    // Reference to the Game Over menu prefab
    [SerializeField]
    private GameObject gameOverMenuPrefab;

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

    
    /// <summary>
    /// Displays the Game Over menu and sets up button listeners.
    /// </summary>
    public void ShowGameOverMenu()
    {
        if (gameOverMenuPrefab == null)
        {
            Debug.LogError("GameOverMenuPrefab is not assigned in the inspector.");
            return;
        }

        // Instantiate the Game Over menu
        GameObject gameOverMenu = Instantiate(gameOverMenuPrefab);

        // Set up the restart button listener
        Button restartButton = gameOverMenu.GetComponentInChildren<Button>();
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        else
        {
            Debug.LogError("No Button component found in the Game Over menu prefab.");
        }
    }

    /// <summary>
    /// Restarts the current game by reloading the active scene.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
