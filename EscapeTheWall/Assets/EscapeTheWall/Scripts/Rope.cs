using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour, Consumable
{

    public void Consume()
    {
        Destroy(gameObject);
    }
}