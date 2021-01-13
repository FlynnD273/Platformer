//Cooper Spring, 12/15/2020, script that allows for frontChecker collider to hold a static bool to be used by the player controller
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrontChecker : MonoBehaviour
{
    public static bool isTouchingFront;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            isTouchingFront = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            isTouchingFront = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger)
        {
            isTouchingFront = false;
        }
    }
}
