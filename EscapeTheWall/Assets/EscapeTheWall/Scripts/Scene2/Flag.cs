using UnityEngine;

public class Flag : MonoBehaviour, Consumable
{
    public void Consume()
    {
        Destroy(gameObject);

        Debug.Log("Finished passing the wall, going to next level...");

        GameManager.Instance.LoadNextLevel();
    }
}
