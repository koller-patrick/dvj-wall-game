using UnityEngine;
using UnityEngine.UI; 

/// <summary>
/// Controls the visibility of a shadow object, its inactive state, and cooldown timer.
/// Provides feedback through a UI element to indicate the current cooldown status.
/// </summary>
public class ShadowController : MonoBehaviour
{
    [Header("Timing Settings")]
    [SerializeField] 
    private float inactiveDuration = 2f; // Time the shadow stays invisible after being deactivated
    [SerializeField] 
    private float cooldown = 5f;        // Time before the player can reactivate the shadow's lights

    [Header("UI Settings")]
    [SerializeField] 
    private TMPro.TextMeshProUGUI cooldownText; // UI text for displaying cooldown information

    private SpriteRenderer spriteRenderer; // Reference to the shadow's sprite renderer
    private float timerToTurnOff; // Timer for how long the shadow stays inactive
    private float timerToCooldown; // Timer for the cooldown phase
    private bool isShadowVisible = true; // Tracks whether the shadow is currently visible
    private bool canActivateLight = true; // Tracks whether the player can activate the shadow's light

    /// <summary>
    /// Initializes the component and timers. Updates the UI to indicate the light is ready for use.
    /// </summary>
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timerToTurnOff = inactiveDuration;
        timerToCooldown = cooldown;

        // Set initial message for the cooldown text UI
        if (cooldownText != null)
        {
            cooldownText.text = "You can use the light (L-Key)!";
        }
    }

    /// <summary>
    /// Handles the shadow's visibility logic, inactive state timing, and cooldown updates.
    /// </summary>
    void Update()
    {
        if (isShadowVisible)
        {
            // Shadow is visible and can potentially be deactivated by the player
            if (!canActivateLight)
            {
                // Cooldown phase: decrease the cooldown timer and update UI text
                if (timerToCooldown > 0)
                {
                    timerToCooldown -= Time.deltaTime;

                    if (cooldownText != null)
                    {
                        cooldownText.text = "Cooldown: " + Mathf.Ceil(timerToCooldown).ToString() + "s";
                    }
                }
                else
                {
                    // Cooldown finished, player can activate the shadow again
                    canActivateLight = true;
                    if (cooldownText != null)
                    {
                        cooldownText.text = "You can use the light (L-Key)!";
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                // Player pressed "L", deactivate the shadow and start the inactive phase
                spriteRenderer.enabled = false; // Make the shadow invisible
                isShadowVisible = false;

                // Reset the inactive timer and start cooldown logic
                timerToTurnOff = inactiveDuration;
                canActivateLight = false;
            }
        }
        else
        {
            // Shadow is inactive; manage inactive timer
            if (timerToTurnOff > 0)
            {
                timerToTurnOff -= Time.deltaTime;
            }
            else
            {
                // Inactive time has passed, make the shadow visible again
                spriteRenderer.enabled = true;
                isShadowVisible = true;

                // Start cooldown logic
                timerToCooldown = cooldown;

                // Update the cooldown UI with the initial cooldown message
                if (cooldownText != null)
                {
                    cooldownText.text = "Cooldown: " + Mathf.Ceil(timerToCooldown).ToString() + "s";
                }
            }
        }
    }
}
