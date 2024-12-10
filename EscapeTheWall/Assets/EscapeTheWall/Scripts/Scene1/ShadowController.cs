using UnityEngine;
using UnityEngine.UI; // Make sure this is included if you're using the legacy UI Text

public class ShadowController : MonoBehaviour
{
    [Header("Timing Settings")]
    [SerializeField] private float inactiveDuration = 2f; // Duration the shadow stays invisible
    [SerializeField] private float cooldown = 5f;        // Duration before player can reactivate lights

    [Header("UI Settings")]
    [SerializeField] 
    private TMPro.TextMeshProUGUI cooldownText; // Assign this in the inspector

    private SpriteRenderer spriteRenderer;
    private float timerToTurnOff;
    private float timerToCooldown;
    private bool isShadowVisible = true;
    private bool canActivateLight = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timerToTurnOff = inactiveDuration;
        timerToCooldown = cooldown;

        // Initially, no cooldown text is displayed
        if (cooldownText != null)
            cooldownText.text = "You can use the light!";
    }

    void Update()
    {
        if (isShadowVisible)
        {
            // Shadow is visible, can attempt to use "L"
            if (!canActivateLight)
            {
                // Show remaining cooldown time as a countdown
                if (timerToCooldown > 0)
                {
                    timerToCooldown -= Time.deltaTime;
                    // Update UI text; show only whole seconds or to one decimal place
                    if (cooldownText != null)
                        cooldownText.text = "Cooldown: " + Mathf.Ceil(timerToCooldown).ToString() + "s";
                }
                else
                {
                    // Cooldown ended, player can activate again
                    canActivateLight = true;
                    // Clear the text
                    if (cooldownText != null)
                        cooldownText.text = "You can use the light!";
                }
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                // Player used L, start inactive phase
                spriteRenderer.enabled = false;
                isShadowVisible = false;

                // Reset timers and start cooldown after shadow reappears
                timerToTurnOff = inactiveDuration;
                canActivateLight = false;
            }
        }
        else
        {
            // Shadow is currently inactive
            if (timerToTurnOff > 0)
            {
                timerToTurnOff -= Time.deltaTime;
            }
            else
            {
                // Make shadow visible again
                spriteRenderer.enabled = true;
                isShadowVisible = true;

                // Begin cooldown phase
                timerToCooldown = cooldown;

                // Start showing the cooldown text again
                if (cooldownText != null)
                    cooldownText.text = "Cooldown: " + Mathf.Ceil(timerToCooldown).ToString() + "s";
            }
        }
    }
}
