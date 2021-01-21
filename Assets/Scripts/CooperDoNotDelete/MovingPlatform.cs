//Cooper Spring, 1/6/2021, Script to control a simple moving platform
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MovingPlatform : MonoBehaviour
{
    [Tooltip("How fast should the platform move if moving?")]
    public float moveSpeed = 1f;
    private bool moveRight = true;
    private float startingPos;

    [Tooltip("How many units should the platform travel in one direction before switching directions or stopping?")]
    public float leftBound;

    [Tooltip("How many units should the platform travel in one direction before switching directions or stopping?")]
    public float rightBound;

    [Tooltip("How long should the player hold down to fall through the platform?")]
    public float waitTimeToFall;
    private float fallWait;

    [Tooltip("Does the platform Move? Moving Platforms cannot be fallen through.")]
    public bool moving = false;

    [Tooltip("Should the platform not move until player has touched the platform?")]
    public bool waitForPlayer = false;
    private bool waiting = true;

    [Tooltip("Freeze Platform if it hits the Left/Right Bounds?")]
    public bool endIfMaxRight = false;

    [Tooltip("Freeze Platform if it hits the Left/Right Bounds?")]
    public bool endIfMaxLeft = false;

    private bool Freeze = false;

    [Tooltip("Should the platform be fall-through? Only non-moving platforms can use fallThrough.")]
    public bool fallThrough;

    //Effector for fallthrough
    private PlatformEffector2D effector;

    private void Start()
    {
        startingPos = transform.position.x;
        effector = gameObject.GetComponent<PlatformEffector2D>();
        fallWait = waitTimeToFall;

    }
    
    private void FixedUpdate()
    {
        //If we don't wait for the player, check for moving normally, else check if we're not waiting before moving
        if (!waitForPlayer)
        {
            if (moving)
            {
                MovePlatform();
            }
        }
        else
        {
            if (moving && !waiting)
            {
                MovePlatform();
            }
        }
        
        //call fallthrough method if fallThrough is true
        if (fallThrough)
        {
            AllowFallThrough();
        }
    }

    private void MovePlatform()
    {
        //check if the platform passes the right bound and decide to stop or switch directions
        if (transform.position.x > (startingPos + rightBound) && !endIfMaxRight)
        {
            moveRight = false;
        }
        else if(transform.position.x > (startingPos + rightBound) && endIfMaxRight)
        {
            Freeze = true;
        }

        //check if the platform passes the left bound and decide to stop or switch directions
        if (transform.position.x < (startingPos - leftBound) && !endIfMaxLeft)
        {
            moveRight = true;
        }
        else if (transform.position.x > (startingPos + rightBound) && endIfMaxLeft)
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
        }
    }
}
