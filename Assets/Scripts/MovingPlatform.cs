//Cooper Spring, 1/6/2021, Script to control a simple moving platform
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MovingPlatform : MonoBehaviour
{
    [Tooltip("How fast should the platform move if moving?")]
    public float moveSpeed = 1f;
    private bool moveRight = true;
    private bool moveUp = true;
    private float startingPosx;
    private float startingPosy;

    [Tooltip("How many units should the platform travel in one direction before switching directions or stopping?")]
    public float leftBound;
    public float upBound;

    [Tooltip("How many units should the platform travel in one direction before switching directions or stopping?")]
    public float rightBound;
    public float downBound;

    [Tooltip("How long should the player hold down to fall through the platform?")]
    public float waitTimeToFall;
    private float fallWait;

    [Tooltip("Does the platform Move? Moving Platforms cannot be fallen through.")]
    public bool platformLeftRight;
    [Tooltip("Does the platform Move? Moving Platforms cannot be fallen through.")]
    public bool platformUpDown;

    [Tooltip("Should the platform not move until player has touched the platform?")]
    public bool waitForPlayer = false;
    private bool waiting = true;

    [Tooltip("Freeze Platform if it hits the Left/Right Bounds?")]
    public bool endIfMaxRight = false;

    [Tooltip("Freeze Platform if it hits the Left/Right Bounds?")]
    public bool endIfMaxLeft = false;

    [Tooltip("Freeze Platform if it hits the Up/Down Bounds?")]
    public bool endIfMaxUp = false;
    [Tooltip("Freeze Platform if it hits the Up/Down Bounds?")]
    public bool endIfMaxDown = false;

    private bool Freeze = false;

    [Tooltip("Should the platform be fall-through? Only non-moving platforms can use fallThrough.")]
    public bool fallThrough = false;

    [Tooltip("Should the platform be one-way? Needs to be enabled if using fallthrough.")]
    public bool oneWay = false;

    [Tooltip("Should the platform return to original position if the player leaves it? Works best if the platform waits for player." +
        " Only should be used in cases where the platform needs to return if the player dies.")]
    public bool returnIfPlayerGone = false;

    [Tooltip("How long should we wait for the player to be off the platform?")]
    public float playerGoneTimerLength = 0f;

    private Stopwatch playerGoneTimer;

    

    //Effector for fallthrough
    private PlatformEffector2D effector;

    private void Start()
    {
        playerGoneTimer = new Stopwatch();

        startingPosx = transform.position.x;
        startingPosy = transform.position.y;
        effector = gameObject.GetComponent<PlatformEffector2D>();
        fallWait = waitTimeToFall;

        if (!oneWay)
        {
            effector.useOneWay = false;
        }

        if(fallThrough && !oneWay)
        {
            UnityEngine.Debug.Log("Fallthrough will not work without oneWay turned on.");
        }
    }
    
    private void FixedUpdate()
    {
        //If we don't wait for the player, check for moving normally, else check if we're not waiting before moving
        if (!waitForPlayer && platformLeftRight)
        {
            MovePlatformLeftRight();
        }
        else if (platformLeftRight && !waiting)
        {
            MovePlatformLeftRight();
        }

        if(platformUpDown && !waitForPlayer)
        {
            MovePlatformUpDown();
        }
        else if (platformUpDown && !waiting)
        {
            MovePlatformUpDown();
        }
        
        //call fallthrough method if fallThrough is true
        if (fallThrough)
        {
            AllowFallThrough();
        }

        if(Freeze && returnIfPlayerGone && playerGoneTimer.ElapsedMilliseconds > (playerGoneTimerLength * 1000))
        {
            Freeze = false;
            waiting = true;
            gameObject.transform.position = new Vector2(startingPosx, startingPosy);
            playerGoneTimer.Reset();
        }
    }

    private void MovePlatformLeftRight()
    {
        //check if the platform passes the right bound and decide to stop or switch directions
        if (transform.position.x > (startingPosx + rightBound) && !endIfMaxRight)
        {
            moveRight = false;
        }
        else if (transform.position.x > (startingPosx + rightBound) && endIfMaxRight)
        {
            Freeze = true;
        }

        //check if the platform passes the left bound and decide to stop or switch directions
        if (transform.position.x < (startingPosx - leftBound) && !endIfMaxLeft)
        {
            moveRight = true;
        }
        else if (transform.position.x < (startingPosx + rightBound) && endIfMaxLeft)
        {
            Freeze = true;
        }

        //Don't move if frozen, allow if not frozen
        if (!Freeze)
        {
            if (moveRight)
            {
                transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
            }
        }

    }

    private void AllowFallThrough()
    {
        //reset the waitTimeToFall if the player stops holding
        if (Input.GetAxisRaw("Vertical") == 0 && !(fallWait == waitTimeToFall))
        {
            fallWait = waitTimeToFall;
        }

        //If we're still holding down and not pressing jump, let the player fall through
        if (Input.GetAxisRaw("Vertical") < 0 && Input.GetAxisRaw("Jump") == 0)
        {
            if (fallWait <= 0)
            {
                effector.rotationalOffset = 180f;

                //set the player's parent to null so they don't keep moving with platform
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.SetParent(null);

                fallWait = waitTimeToFall;
            }
            else if (fallWait > 0f)
            {
                fallWait -= Time.deltaTime;
            }
        }

        //reset the rotational offset when jumping
        if (Input.GetAxisRaw("Jump") > 0)
        {
            effector.rotationalOffset = 0f;
        }

    }

    private void MovePlatformUpDown()
    {
        //check if the platform passes the right bound and decide to stop or switch directions
        if (transform.position.y > (startingPosy + upBound) && !endIfMaxUp)
        {
            moveUp = false;
        }
        else if (transform.position.y > (startingPosy + upBound) && endIfMaxUp)
        {
            Freeze = true;
        }

        //check if the platform passes the left bound and decide to stop or switch directions
        if (transform.position.y < (startingPosy - downBound) && !endIfMaxDown)
        {
            moveUp = true;
        }
        else if (transform.position.y < (startingPosy + downBound) && endIfMaxDown)
        {
            Freeze = true;
        }

        //Don't move if frozen, allow if not frozen
        if (!Freeze)
        {
            if (moveUp)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(gameObject.transform, true);
            waiting = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
            //Only start the timer to reset the platform if the platform isn't already frozen. 
            //This fixes situations where you only want the platform to reset if the player has to respawn and get on it again.
            if (!Freeze)
            {
                playerGoneTimer.Start();
            }
        }
    }

}
