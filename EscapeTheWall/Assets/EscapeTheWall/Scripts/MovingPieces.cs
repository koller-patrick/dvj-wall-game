using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class MovingPieces : MonoBehaviour{



    protected GameObject WhatInThisPosition(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(position);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                return collider.gameObject;
            }
        }

        return null;
    }


}
