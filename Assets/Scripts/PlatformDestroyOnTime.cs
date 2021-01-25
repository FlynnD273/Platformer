/********
 * Arya Khubcher
 * 1/24/21
 * Desc: After setting this on a platform, when that platform comes in contact with the player, the platform will be destroyed in the seconds it is set to.
 *////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDestroyOnTime : MonoBehaviour
{
    // setting the time before the platform is destroyed
    public float TimeBeforeDestroy;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // after  the platform collides with the player, then destroy the gameObject in the time set for it to happen
        if(collision.gameObject.tag == ("Player"))
        {
            // destroy the gameObject after the certain amount of time it is set to
            Destroy(gameObject, TimeBeforeDestroy);
        }
    }
}
