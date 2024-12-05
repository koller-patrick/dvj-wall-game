using UnityEngine;

public class GuardVision : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform[] waypoints;

    private int waypointIndex = 0;

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        // TODO: guard should move
    }
}
