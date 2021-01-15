/********
 * Arya Khubcher
 * 1/13/21
 * Desc: when this is added to the trigger, when the trigger is activated, then shoot arrows in random directions.
 *////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AryaArrowTriggerShoot : MonoBehaviour
{
    //public GameObject arrows;
    public GameObject player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Instantiate(arrows, player.transform.position, Quaternion.identity);
        }
    }
}
