using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    private bool gameEnded = false;
    private bool isPaused = false;  

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


}
