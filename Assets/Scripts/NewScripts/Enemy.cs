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
    public GameObject proj2;
    public int dropRate = 5;
    public bool isEnemy = true;
    private int randomRate;

    //enemy Movement
    [Tooltip("These points treat 0, 0, 0 as the start point, all other points are relative to that.")]
    public Vector3[] waypoints = { Vector3.zero, new Vector3(4, 0, 0) };
    private int currentWayPoint = 0;
    public float speed = 1;
    public float closeEnough = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        //convert the waypoints into world space
        for (int i = 0; i < waypoints.Length; ++i)
        {
            waypoints[i] += transform.position;
        }
    }

    void FixedUpdate()
    {
        if (isEnemy == true)
        {
            //get a vector to the next waypoint from our position
            Vector3 toWaypoint = waypoints[currentWayPoint] - transform.position;
            //check if we are close enough to there
            if (toWaypoint.sqrMagnitude < closeEnough * closeEnough)
            {
                //if so target next waypoint
                ++currentWayPoint;
                //if end reset to 0th waypoint
                if (currentWayPoint >= waypoints.Length)
                {
                    currentWayPoint = 0;
                }
                //recalculate the vector
                toWaypoint = waypoints[currentWayPoint] - transform.position;
            }
            //normalize vector to the next waypoint, so we can scale correctly
            toWaypoint.Normalize();
            //move the platfrom
            transform.position += toWaypoint * speed * Time.fixedDeltaTime;
        }
    }

    //when function is called, record the damage and check if the object should be destroyed
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Death();
        }
    }
    //handle spawning of death effects and destroy gameobject
    void Death()
    {
        if (isEnemy == true)
        {
            randomRate = Random.Range(1, dropRate);
            for (int i = 0; i < randomRate; i++)
            {
                Instantiate(proj1, transform.position, transform.rotation);
            }
            randomRate = Random.Range(1, dropRate);
            for (int i = 0; i < randomRate; i++)
            {
                Instantiate(proj2, transform.position, transform.rotation);
            }
        }
        else
        {
            for (int i = 0; i < dropRate; i++)
            {
                Instantiate(proj1, transform.position, transform.rotation);
            }
        }
        //if(isEnemy == true)
        //{
            //Instantiate(deathEffect, transform.position, Quaternion.identity);
        //}
        Destroy(gameObject);
    }


}