using UnityEngine;

public class ShadowController : MonoBehaviour
{   
    [SerializeField]
    private float activeDuration = 5f;  // Duration the shadow stays visible
    [SerializeField]
    public float inactiveDuration = 1f; // Duration the shadow stays invisible

    private SpriteRenderer spriteRenderer;
    private float timer;
    private bool isShadowVisible = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = activeDuration;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (isShadowVisible && timer <= 0)
        {
            // Make the shadow invisible after 10 seconds
            spriteRenderer.enabled = false;
            isShadowVisible = false;
            timer = inactiveDuration; // Set timer for the inactive period
        }
        else if (!isShadowVisible && timer <= 0)
        {
            // Make the shadow visible again after 5 seconds
            spriteRenderer.enabled = true;
            isShadowVisible = true;
            timer = activeDuration; // Set timer for the active period
        }
    }
}
