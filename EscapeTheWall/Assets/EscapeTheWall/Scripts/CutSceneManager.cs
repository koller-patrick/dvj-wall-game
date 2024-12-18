using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{

    [Header("Audio Settings")]
    // Reference to the AudioSource component
    [SerializeField]
    private AudioClip cutSceneAudio;

    [SerializeField]
    private float timer = 19;


    // Start is called before the first frame update
    void Start()
    {
        // Ensure the audio source is assigned and start playing
        if (cutSceneAudio == null)
        {
            Debug.LogError("AudioSource is not assigned in the inspector.");
        }
        else
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.clip = cutSceneAudio;
                audioSource.Play();
            }
            else
            {
                Debug.LogError("AudioSource component not found on CutSceneManager object.");
            }
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            // Load the next scene
            SceneManagerETW.Instance.LoadNextScene();
        }
    }

    public void SkipCutScene()
    {
        // Load the next scene
        SceneManagerETW.Instance.LoadNextScene();
    }
}
