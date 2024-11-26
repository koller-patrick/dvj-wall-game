using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    public static T instance { get; private set; }

    private void Awake() {
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        } else {
            instance = this as T;
        }
    }

}
