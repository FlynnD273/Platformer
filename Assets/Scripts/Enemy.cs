﻿//////////////////
//By: Dev Dhawan
//Date: 1/12/2020
//Description: Enemy logic fpr 2D game.
//////////////////
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //enemy health
    public int health = 100;
    private int temp;
    private Vector3 healthT;

    //gameobjects for the effect to be spawned when the enemy dies
    public GameObject deathEffect;
    public GameObject proj1;
    public GameObject proj2;
    public int dropRate = 5;
    public bool isEnemy = true;
    public bool dropsAmmo = true;
    public bool shootingTypeEnemy = true;
    private int randomRate;
    public Transform healthBar;

    //enemy Movement
    [Tooltip("These points treat 0, 0, 0 as the start point, all other points are relative to that.")]
    public Vector3[] waypoints = { Vector3.zero, new Vector3(4, 0, 0) };
    private int currentWayPoint = 0;
    public float speed = 1;
    public float closeEnough = 0.1f;
    private bool flip;

    // Start is called before the first frame update
    void Start()
    {
        //convert the waypoints into world space
        for (int i = 0; i < waypoints.Length; ++i)
        {
            waypoints[i] += transform.position;
            healthT = healthBar.transform.localScale;
        }
    }
    void Update()
    {
        float temp = health * 0.01f;
        healthT = new Vector3(temp, 0.0875f, 0f);
        healthBar.transform.localScale = healthT;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Changed by Cooper, redundant and causes double hits of damage
        /*if (collision.gameObject.CompareTag("Sword"))
        {
            TakeDamage(15);
            Debug.Log("Hit");
        }*/
        flip = true;
    }

    void FixedUpdate()
    {
        if (isEnemy)
        {
            //get a vector to the next waypoint from our position
            Vector3 toWaypoint = waypoints[currentWayPoint] - transform.position;
            //check if we are close enough to there
            if (toWaypoint.sqrMagnitude < closeEnough * closeEnough)
            {
                //if so target next waypoint
                ++currentWayPoint;
                //if end reset to 0th waypoint
                if (shootingTypeEnemy == false)
                {
                    if (currentWayPoint >= waypoints.Length)
                    {
                        Flip();
                        flip = true;
                        Debug.Log(flip);
                        currentWayPoint = 0;
                    }
                    else
                    {
                        Flip();
                        flip = false;
                        Debug.Log(flip);
                    }
                }
                else
                {
                    if (currentWayPoint >= waypoints.Length)
                    {
                        currentWayPoint = 0;
                    }
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
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        if (dropsAmmo)
        {
            if (isEnemy)
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
        }
        
        Destroy(gameObject);
    }
    void Flip()
    { 
        transform.Rotate(0, 180, 0);
    }
    
    public bool IsFlip()
    {
        if(flip == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}