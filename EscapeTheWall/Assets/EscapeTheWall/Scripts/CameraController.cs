using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Material maskMaterial;
    public Transform player;

    void Update()
    {
        // Calculate player's position in viewport coordinates
        Vector3 playerViewportPos = Camera.main.WorldToViewportPoint(player.position);
        maskMaterial.SetVector("_Center", new Vector4(playerViewportPos.x, playerViewportPos.y, 0, 0));
    }

}
