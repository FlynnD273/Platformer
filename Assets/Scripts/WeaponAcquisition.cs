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
            ProjectileHandler.maxWeapons++; //raise thier max weapons
            Destroy(gameObject); //get rid of the pcikup
        }
    }



}
