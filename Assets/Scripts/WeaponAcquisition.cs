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
    public bool shuriken;
    public bool fireball;
    public bool bigFireball;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //make sure it's the player
        {
            //i know i should do thsi with a ++ but it was triggering multiple times so yeah
            if (shuriken)
            {
                ProjectileHandler.maxWeapons = 2;
                Destroy(gameObject); //get rid of the pcikup
            }
            else if (fireball)
            {
                ProjectileHandler.maxWeapons = 3;
                Destroy(gameObject); //get rid of the pcikup
            }
            else if (bigFireball)
            {
                ProjectileHandler.maxWeapons = 4;
                Destroy(gameObject); //get rid of the pcikup
            }
            

        }
    }



}
