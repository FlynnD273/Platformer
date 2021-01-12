//////////////////
//By: Dev Dhawan
//Date: 12/17/2020
//Description: Enemy logic fpr 2D game.
//////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //enemy health
    public int health = 100;
    //gameobjects for the effect to be spawned when the enemy dies
    public GameObject deathEffect;
    public GameObject proj1;

    public int dropRate = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //when function is called, record the damage and check if the object should be destroyed
    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Death();
        }
    }
    //handle spawning of death effects and destroy gameobject
    void Death()
    {
        for(int i = 0; i < dropRate; i++)
        {
            Instantiate(proj1, transform.position, transform.rotation);
        }
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
