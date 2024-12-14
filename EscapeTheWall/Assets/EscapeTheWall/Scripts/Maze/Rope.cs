using UnityEngine;

public class Rope : MonoBehaviour, Consumable
{
    public void Consume()
    {
        Destroy(gameObject);

        Debug.Log("Collected rope, going to next level...");

        // reached goal, so move to next level
        SceneManagerETW.Instance.LoadNextScene();
    }
}