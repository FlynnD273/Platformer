/*
 * Name : Cooper Spring, Thomas Allen and Abhi 
 * Date : 12/14/2020 
 * Description : Script to control player movement in a 2D platformer game
 */

using UnityEngine;
using System.Diagnostics;
using UnityEngine.Events;

public class CooperPlayerController : MonoBehaviour
{
    [Header("Player Movement Values")]
    //speed
    public float speed;
    //jump
    public float jumpForce;
    public float jumpTime;
    public int maxJumps;
    public float fallMultiplier = 1.5f;
    //wall
    public float wallJumpTime;
    public float wallSlideTime;
    public float wallSlidingSpeed;
    public float xWallForce;
    public float yWallForce;
    //ladder
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] bool onLadder;
    [SerializeField] bool climbingLadder;
    //adds a delay before sliding happens when player is on clinging to the wall
    public Stopwatch wallSlideStopwatch;

    [Header("Boolean Values for debugging")]
    [SerializeField] private bool wallSliding = false;
    [SerializeField] private bool invertInput = false;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool wallJumping = false;
    [SerializeField] public bool isTouchingWall;
    [SerializeField] public bool isTouchingGround;

    
    private float wallJumpTimeMax;
    private float storedMoveInput;
    /// <summary>
    /// The number of extra jumps in the air
    /// </summary>
    private int extraJumps;
    private float jumpTimeCounter;
    private bool facingRight = true;
    private float moveInput;
    private bool prevJump;

    //initialisisng gameobject component
    private Rigidbody2D myRB;
    private Animator myAnim;


    [Header("Sound")]
    public AudioClip jumpSound;
    private AudioSource jumpSource;

    PolygonCollider2D frontCheck;
    CapsuleCollider2D groundCheck;


    private Vector3 stupidFix;
    // Start is called before the first frame update
    void Start()
    {
        //get components from player gameobject
        myRB = gameObject.GetComponent<Rigidbody2D>();
        myAnim = gameObject.GetComponent<Animator>();
        jumpSource = gameObject.GetComponent<AudioSource>();
        //set max time to public var
        wallJumpTimeMax = wallJumpTime;
        wallSlideStopwatch = new Stopwatch();
        frontCheck = GetComponent<PolygonCollider2D>();
        groundCheck = GetComponent<CapsuleCollider2D>();
    }

    //update is called once per frame (needed for key checks)
    void Update()
    {
        myAnim.SetBool("WallSlide", wallSliding);
        ClimbLadder();
        if (!wallSliding)
        {
            //reset the stopwatch if not wallsliding
            wallSlideStopwatch.Reset();
        }

        //stop holding the player in place if they stop holding horizontally
        if (wallSliding && Input.GetAxisRaw("Horizontal") == 0)
        {
            myRB.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            wallSliding = false;
            wallSlideStopwatch.Reset();
        }

        //stop inverting input if the player stops holding a direction so the player has responsive control
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            invertInput = false;
            myRB.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }

        if(Input.GetAxisRaw("Jump") > 0)
        {
            myRB.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        }

        //check for wall slide/ wall jump when moving into a wall above ground, check for normal jumps if criteria not met
        if (isTouchingWall && !isTouchingGround && moveInput != 0)
        {
            WallSlideCheck();
        }
        else if(!wallJumping)
        {
            //Stop inverting input for wall jump chains and falsify wallsliding in case it doesn't get falsified earlier
            if (!isTouchingWall)
            {
                invertInput = false;
                wallSliding = false;
            }

            JumpCheck();
        }

        //use walljump timer while wall jumping to control the length
        if (wallJumping)
        {
            //Reset the stopwatch on walljump
            wallSlideStopwatch.Reset();
        }

        if (myRB.velocity.y < 0 || myRB.velocity.y > 1)
        {
            myRB.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        prevJump = Input.GetAxisRaw("Jump") > 0;
    }

    // Update is called once per physics frame
    void FixedUpdate()
    {
        //check if a wall has been hit every physics frame while wall jumping
        if (wallJumping)
        {
            Invoke(nameof(CheckForNewWallHit), 0f);
        }

        myAnim.SetBool("OnGround", isTouchingGround);
        myAnim.SetBool("IsFalling", myRB.velocity.y < 0);

        moveInput = Input.GetAxisRaw("Horizontal");

        //allows player to hold one direction while wall jumping, making timing easier
        if (invertInput)
        {
            //invert the player's input
            moveInput *= -1;
        }

        //set wall jump bool to false when player hits ground or another wall if function is not invoked by then
        if (isTouchingGround)
        {
            wallJumping = false;
        }

        //don't let player input interfere while wall jumping
        if (!wallJumping && !(wallSliding || invertInput))
        {
            //set velocity for left right movement
            myRB.velocity = new Vector2(moveInput * speed, myRB.velocity.y);
        }

        myAnim.SetBool("Walking", moveInput != 0);

        //when player isn't wall jumping or sliding, allow for 
        
        //ping to occur with movement
        if (!wallJumping && !wallSliding)
        {
            if (!facingRight && moveInput > 0)
            {
                Flip();
            }
            else if (facingRight && moveInput < 0)
            {
                Flip();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (frontCheck.IsTouching(collision))
        {
            if (!collision.isTrigger && collision.CompareTag("Tilemap"))
            {
                isTouchingWall = true;
            }
        }
        if (groundCheck.IsTouching(collision) && collision.CompareTag("Tilemap"))
        {
            if (!collision.isTrigger)
            {
                isTouchingGround = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (frontCheck.IsTouching(collision) && collision.CompareTag("Tilemap"))
        {
            if (!collision.isTrigger)
            {
                isTouchingWall = true;
            }
        }
        if (groundCheck.IsTouching(collision) && collision.CompareTag("Tilemap"))
        {
            if (!collision.isTrigger)
            {
                isTouchingGround = true;
            }
        }
    
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!frontCheck.IsTouching(collision) && collision.CompareTag("Tilemap"))
        {
            if (!collision.isTrigger)
            {
                isTouchingWall = false;
            }
        }
        if (!groundCheck.IsTouching(collision) && collision.CompareTag("Tilemap"))
        {
            if (!collision.isTrigger) {
                isTouchingGround = false;
            }
        }
    }

    //Flip the sprite based on movement direction
    void Flip() 
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    //function to time a walljump, sets walljumping to false when timer reaches 0
    /*void WallJumpTimer()
    {
        if (wallJumpTime <= 0)
        {
            wallJumping = false;
            wallJumpTime = wallJumpTimeMax;
        }

        wallJumpTime -= Time.deltaTime;
    }*/

    
    //function that checks if there is wall the player is touching
    bool CheckForNewWallHit()
    {
        if (isTouchingWall)
        {
            wallJumping = false;
            return true;
        }
        return false;
    }
    
    public void ClimbLadder()
    {
        onLadder = true;
      
        myAnim.SetBool("OnLadder", onLadder);
        if (!groundCheck.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {
            onLadder = false;
            climbingLadder = false;
            myAnim.SetBool("OnLadder", onLadder);
            myAnim.SetBool("ClimbingLadder", onLadder);
            myRB.gravityScale = 1; ;
            return;
        }

        if(groundCheck.IsTouchingLayers(LayerMask.GetMask("Ladder"))){
            jumpSource.Stop();
        }
            float moveUp = Input.GetAxis("Vertical");
            Vector2 climbVel = new Vector2(myRB.velocity.x, moveUp * climbSpeed);
            myRB.velocity = climbVel;
            myRB.gravityScale = 0;
            bool verSpeed = Mathf.Abs(myRB.velocity.y) > Mathf.Epsilon;

            climbingLadder = verSpeed;

            myAnim.SetBool("ClimbingLadder", climbingLadder);
            
       



    }

    //check conditions for jumping and handle jumping
    void JumpCheck()
    {
        //refresh jumps when on ground
        if (isTouchingGround)
        {
            invertInput = false;
            extraJumps = maxJumps;
        }

        //activate jump if jumps remain
        if (Input.GetAxisRaw("Jump") > 0 && !prevJump && extraJumps > 0)
        {
            if (extraJumps != maxJumps || isTouchingGround)
            {
                isJumping = true;

                jumpSource.PlayOneShot(jumpSound);

                //reset jumptimecounter to the initial value to refresh it for the current jump
                jumpTimeCounter = jumpTime;

                //apply movement and lower jump var
                myRB.velocity = Vector2.up * jumpForce;
                extraJumps--;
            }
        }

        //allow player to jump higher within a timeframe as long as key is pressed
        if (Input.GetAxisRaw("Jump") > 0 && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                myRB.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        //when the key is released, set isJumping to false
        if (Input.GetAxisRaw("Jump") <= 0)
        {
            isJumping = false;
        }
    }

    //check conditions for wall sliding and handle it as well as wall jumping
    void WallSlideCheck()
    {
        //set wallslide to true if parameters are met and vice versa
        if (isTouchingWall)
        {
            wallSliding = true;

            if (!wallSlideStopwatch.IsRunning)
            {
                wallSlideStopwatch.Start();
            }
        }
        else
        {
            wallSliding = false;
        }

        //if player is wall sliding, apply relevant velocity change
        if (wallSliding)
        {
            //check if time is up for the stopwatch and apply sliding, if not, then keep player in place
            if (wallSlideStopwatch.IsRunning && wallSlideStopwatch.ElapsedMilliseconds > wallSlideTime * 1000)
            {
                myRB.velocity = new Vector2(myRB.velocity.x, Mathf.Clamp(myRB.velocity.y, -wallSlidingSpeed, float.MaxValue));
                myRB.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                myRB.velocity = new Vector2(0,0);

                myRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

                //If the player inputs to walljump, unfreeze constraints before the next if statement
                if (isTouchingWall && Input.GetAxisRaw("Jump") > 0)
                {
                   myRB.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation; 
                }
            }
        }
        
        //if player presses key while wall sliding, set wall jumping to true and set it to false after invoke time
        if (Input.GetAxisRaw("Jump") > 0 && !prevJump && isTouchingWall)
        {
            myRB.velocity = new Vector2(myRB.velocity.x, Mathf.Clamp(myRB.velocity.y, -wallSlidingSpeed, float.MaxValue));
            myRB.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

            //check which direction the walljump should be in
            storedMoveInput = facingRight ? -1 : 1;

            //flip the player before the next movement
            Flip();

            jumpSource.PlayOneShot(jumpSound);

            //start inverting input, this is meant to allow for the player to do consecutive wall jumps while holding one directional input
            //if the player changes direction, input will stop being inverted
            invertInput = !invertInput;

            //wall jumping removes extra jumps, this can be changed if desired
            extraJumps = 0;

            //set walljumping to true so we can apply the walljumping movement
            wallJumping = true;

            //while the player is in a wall jump chain, don't start the timer for ending the wall jump
            wallJumpTime = wallJumpTimeMax;
        }

        //if wall jumping, apply the relevant movement
        if (wallJumping)
        {
            //stop inverting input if the player changes input directions while wall jumping
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                invertInput = false;
            }

            myRB.velocity = new Vector2(xWallForce * storedMoveInput, yWallForce);
        }

        //check if we're wallsliding still after the wall jump for jump chains
        if (!isTouchingWall)
        {
            wallSliding = false;
        }
    }
}

