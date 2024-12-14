using UnityEngine;

/// <summary>
/// Manages the overall game state, including the timer, game over logic, and level transitions.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    // Indicates whether the timer is active
    private bool timerIsRunning = false;

    // Reference to the TextMeshProUGUI component for displaying the timer
    [SerializeField]
    private TMPro.TextMeshProUGUI timerText;

    // Time remaining for the game
    [SerializeField]
    private float timeRemaining = 30f;

    // Indicates whether the game has ended
    private bool hasGameEnded = false;


    /// <summary>
    /// Initializes the game timer and state.
    /// </summary>
    private void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("TimerText is not assigned in the inspector.");
            return;
        }

        timerText.text = "Time: 30";
        timerIsRunning = true;
    }

    /// <summary>
    /// Updates the game timer and handles game-over logic.
    /// </summary>
    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                // Update the timer display and decrement the remaining time
                timerText.text = "Time: " + timeRemaining.ToString("F0");
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                // Time is up
                timeRemaining = 0;
                timerIsRunning = false;
                hasGameEnded = true;
                timerText.text = "Game over!";
                SceneManagerETW.Instance.ShowGameOverMenu();
            }
        }
    }

    /// <summary>
    /// Checks if the game is over.
    /// </summary>
    /// <returns>True if the game has ended; otherwise, false.</returns>
    public bool IsGameOver()
    {
        return hasGameEnded;
    }

    /// <summary>
    /// Sets the game state to "Game Over" and displays the Game Over menu.
    /// </summary>
    public void SetGameOver()
    {
        hasGameEnded = true;
        timerIsRunning = false;

        if (timerText != null)
        {
            timerText.text = "";
        }

        SceneManagerETW.Instance.ShowGameOverMenu();
    }
}

