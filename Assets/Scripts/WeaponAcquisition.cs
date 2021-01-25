//////////////////////
///Name: Thomas Allen
///Date: 1/22/21
///desc: put this in a pick up item to allow the player to use their next weapon
/////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAcquisition : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //make sure it's the player
        {
            //i know i should do thsi with a ++ but it was triggering multiple times so yeah
            if (ProjectileHandler.maxWeapons < 1)
                ProjectileHandler.maxWeapons = 1;
            else if (ProjectileHandler.maxWeapons < 2)
                ProjectileHandler.maxWeapons = 2;
            else if (ProjectileHandler.maxWeapons < 3)
                ProjectileHandler.maxWeapons = 3;
            else if (ProjectileHandler.maxWeapons < 4)
                ProjectileHandler.maxWeapons = 4;
            Destroy(gameObject); //get rid of the pcikup
        }
    }



}
