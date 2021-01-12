//////////////////
//By: Dev Dhawan
//Date: 1/11/2020
//Description: Projectile Logic for 2D game.
//////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //where the projectile should be fired from
    public Transform firePoint;

    //gameobject used as the main projectile to be fired
    public GameObject proj1;
    public ProjectileMotion projectile;
    //counter for the amount of kunai the user has
    public int kunai = 20;
    //the max amount of kunai the user can have
    public int addKunai = 20;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //check if player has enough kunai and has pressed space
        if (Input.GetKeyDown(KeyCode.Space) && kunai <= 0)
        {
            kunai--;
            Fire();
        }
    }

    //spawn the kunai
    void Fire ()
    {
        //projectile logic
        Instantiate(proj1, firePoint.position, firePoint.rotation);
    }

    //function that increases kunai by a specified amount
    public void IncreaseKun(int amount)
    {
        kunai += amount;
    }
}
