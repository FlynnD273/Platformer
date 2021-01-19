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
    public GameObject projectile;
    public GameObject player;
    public float Speed; 
    public void Start()
    {
        //set speed of projectile to be a random number between 0 and 10
        Speed = Random.Range(1, 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            //If there is a collision with the player, then shoot the projectile at it with a random speed
            GetComponent<Rigidbody2D>().velocity = (Vector2)transform.right * Speed;
            Instantiate(projectile, player.transform.position, Quaternion.identity);
        }
    }
}
