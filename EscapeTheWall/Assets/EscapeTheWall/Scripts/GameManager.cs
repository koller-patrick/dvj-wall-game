using UnityEngine;

/// <summary>
/// Manages the overall game state, including the timer, game over logic, and level transitions.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    [Header("Timer Settings")]
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

    [Header("Audio Settings")]
    // Reference to the AudioSource component
    [SerializeField]
    private AudioClip gameAudio;

    /// <summary>
    /// Initializes the game timer, state, and audio.
    /// </summary>
    private void Start()
    {
        // Ensure the timer text is assigned
        if (timerText == null)
        {
            Debug.LogError("TimerText is not assigned in the inspector.");
            return;
        }

        timeRemaining -= SceneManagerETW.Instance.GetDifficulty() * 7;

        Debug.Log("Time remaining: " + timeRemaining);

        // Initialize the timer text and state
        timerText.text = "Time: --";
        timerIsRunning = true;

        // Ensure the audio source is assigned and start playing
        if (gameAudio == null)
        {
            Debug.LogError("AudioSource is not assigned in the inspector.");
        }
        else
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.clip = gameAudio;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("AudioSource component not found on GameManager object.");
            }
        }
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
