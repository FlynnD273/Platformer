//Cooper Spring, 1/22/2021, script for controlling a kunai trap
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Events;

public class KunaiTrap : MonoBehaviour
{
    [Header("Projectile")]
    [Tooltip("The object to be used as the trap projectile")]
    public GameObject Kunai;
    [Tooltip("How fast does the trap shoot?")]
    public float fireRate;
    [Header("Fire Direction")]
    [Tooltip("Fire from the left or right direction?")]
    public bool fireLeft;
    [Tooltip("Fire from the left or right direction?")]
    public bool fireRight;
    
    private bool canSeePlayer = false;
    private Vector3 firePos;
    private Quaternion kunaiRotation;
    private Stopwatch trapFireStopwatch;

    // Start is called before the first frame update
    void Start()
    {
        //initialize stopwatch
        trapFireStopwatch = new Stopwatch();
        
        //Safety net if the stopwatch doesn't initialize for some reason. This is probably ok to remove, but I put it here just in case.
        if (!trapFireStopwatch.IsRunning)
        {
            trapFireStopwatch.Start();
            trapFireStopwatch.Stop();
        }
        
        //Check which direction the trap is firing, change the rotation of the kunai that will be spawned and the place where it's fired from
        if (fireLeft)
        {
            kunaiRotation = Quaternion.Euler(0,0,180);
            firePos = new Vector3((transform.position.x - 0.5f), transform.position.y, transform.position.z);
        }
        else if (fireRight)
        {
            kunaiRotation = Quaternion.Euler(0,0,0);
            firePos = new Vector3((transform.position.x + 0.5f), transform.position.y, transform.position.z);
        }
        else if(fireLeft && fireRight)
        {
            //handle unexpected config
            UnityEngine.Debug.Log(gameObject.name + "Has both the left and right boolean checked. It will not function.");
        }
        else
        {
            //handle unexpected config
            UnityEngine.Debug.Log(gameObject.name + "Does not have a left or right boolean checked. It will not function.");
        }
        

        
    }

    // Update is called once per frame
    void Update()
    {
        //Reset the timer if we can't see the player and it's still running.
        if (!canSeePlayer && trapFireStopwatch.IsRunning)
        {
            trapFireStopwatch.Reset();
        }

        //Start the fire rate stopwatch if we can see the player and it isn't running yet
        if (canSeePlayer)
        {
            if (!trapFireStopwatch.IsRunning)
            {
                trapFireStopwatch.Start();
            }
        }

        //If we can see the player and enough time has passed in the stopwatch, instantiate a kunai
        if (canSeePlayer && trapFireStopwatch.ElapsedMilliseconds > (fireRate * 1000))
        {
            Instantiate(Kunai, firePos, kunaiRotation);

            //restart the stopwatch
            trapFireStopwatch.Restart();
        }
    }

    //Handle the trigger collision for detecting the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canSeePlayer = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canSeePlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canSeePlayer = false;
        }
    }
}
