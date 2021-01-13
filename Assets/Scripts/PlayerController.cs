/********************************
 * Author: Flynn Duniho
 * Date: 12/18/2020
 * Purpose: Handles player movement and animation
********************************/

using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Player;
    public float WalkSpeed = 10;
    public float JumpSpeed = 20;
    public float FloatForce = 0.2f;
    public float FloatDuration = 1f;
    public float XDecay = 0.9f;
    private float xSpeed;
    public int MaxJump = 2;
    public AudioClip Jump;
    bool jump;

   
    [SerializeField]bool wallSliding = false;
    bool invertInput = false;
    public float wallSlidingSpeed;
    [SerializeField]bool wallJumping = false;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTimeMax;
    private float storedMoveInput;
    private bool facingRight = true;

    bool onGround = false;

    public bool OnPlatform { get; set; }
    public Rigidbody2D Platform { get; set; }
    public Vector2 LastPlatform { get; set; }
    //private bool onPlatformLastFrame;



    public Vector2 StartPos { get; set; }

    public UnityEvent Die = new UnityEvent();


    private Rigidbody2D rb;

    private float xScale;
    private bool prevJumped = true;
    private int jumps = 0;
    private Stopwatch FloatTime;
    private float wallJumpTime;
    private AudioSource Audio;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        xScale = transform.localScale.x;
        FloatTime = new Stopwatch();
        Die.AddListener(Reset);
        StartPos = rb.position;
        Player = this;
        Audio = GetComponent<AudioSource>();
        //set max time to public var
        wallJumpTime = wallJumpTimeMax;
    }

    private void Update()
    {
        //stop inverting input if the player stops holding a direction so the player has responsive control
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            invertInput = false;
        }

        //check for wall slide/ wall jump when moving into a wall above ground, check for normal jumps if criteria not met
        if (PlayerFrontChecker.isTouchingFront == true && onGround == true && Input.GetAxisRaw("Horizontal") != 0)
        {
            WallSlideCheck();
        }
        
        if (!wallJumping)
        {
            //Stop inverting input for wall jump chains and falsify wallsliding in case it doesn't get falsified earlier
            if (!PlayerFrontChecker.isTouchingFront)
            {
                invertInput = false;
                wallSliding = false;
            }

        }

        //use walljump timer while wall jumping to control the length
        if (wallJumping)
        {
            WallJumpTimer();
            
        }

        WallSlideCheck();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check if a wall has been hit every physics frame while wall jumping
        if (wallJumping)
        {
            Invoke(nameof(CheckForNewWallHit), 0f);
        }

        //check what the player is colliding with and determine if the player is colliding with the ground or not
        onGround = false;
        Collider2D[] groundCollide = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.8f, 0.2f), 0);
        foreach (Collider2D c in groundCollide)
        {
            if (c.CompareTag("Ground"))
            {
                onGround = true;
                //set wall jump bool to false when player hits ground or another wall if function is not invoked by then
                SetWallJumpingToFalse();
                break;
            }
        }


        //set animation to grounded animation depending on the onGround bool
        anim.SetBool("OnGround", onGround);

        //check if the player can jump
        jump = false;
        if (onGround && !wallJumping && !wallSliding)
        {
            //refresh jumps and make player have more drag on ground
            jumps = MaxJump - 1;
            rb.drag = 2;
        }
        else
        {
            //if in air, set falling animation and change drag
            rb.drag = 1f;
            anim.SetBool("IsFalling", rb.velocity.y < 0);
        }

        //if player inputs jump, handle that from there
        if (Input.GetAxisRaw("Jump") > 0.5f && !wallJumping && !wallSliding)
        {
            //check if jump is valid
            if (jumps > 0 && !prevJumped)
            {
                //subtract from jump counter once jumped
                if (!onGround)
                    jumps--;
                //set jump to true, play sound effect, and restart floating time
                jump = true;
                Audio.PlayOneShot(Jump, 0.6f);
                FloatTime.Restart();
            }
            //set prevJumped to true so we know that the player has inputted jump already
            prevJumped = true;

            //as long as the player is holding jump, apply movement for how long it's been set to apply
            if (FloatTime.IsRunning && FloatTime.ElapsedMilliseconds < FloatDuration * 1000)
            {
                rb.AddForce(new Vector2(0, FloatForce));
            }
        }
        else
        {
            //If the player isn't inputting jump, stop the float timer and set prevjumped to false
            prevJumped = false;
            FloatTime.Stop();
        }

        
        //set directional input float
        if (!wallJumping)
        {
            xSpeed = Input.GetAxisRaw("Horizontal") * WalkSpeed;
        }
        

        //moves the player up if jumping, doesn't do that if not 
        rb.velocity = new Vector2(rb.velocity.x * XDecay, jump ? JumpSpeed : rb.velocity.y);

        //allows player to hold one direction while wall jumping, making timing easier
        if (invertInput == true)
        {
            //invert the player's input
            xSpeed = -(Input.GetAxisRaw("Horizontal") * WalkSpeed);
        }
        else if (!invertInput)
        {
            //get input normally
            xSpeed = Input.GetAxisRaw("Horizontal") * WalkSpeed;
        }

        //applies player input to rb if not walljumping
        if (!wallJumping)
        {
            rb.AddForce(new Vector2(xSpeed, 0));
        }
        

        //if the player is falling, stop the float timer
        if (rb.velocity.y < 0)
        {
            FloatTime.Stop();
        }

        //handle player flipping (change directions)
        if (xSpeed != 0 && !wallJumping && !wallSliding)
        {
            if (!facingRight && xSpeed > 0)
            {
                Flip();
            }
            else if (facingRight && xSpeed < 0)
            {
                Flip();
            }
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }

        if (OnPlatform && Platform != null)
        {
            rb.position += Platform.position - LastPlatform;
            LastPlatform = Platform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //kill player if it touches dangerous object
        if (col.CompareTag("Dangerous"))
        {
            Die.Invoke();
        }
    }

    public void Reset()
    {
        rb.position = StartPos;
        prevJumped = true;
        rb.velocity = Vector2.zero;
    }

    //set the wall jumping bool to false after invoke method calls function
    void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }

    
    //function that checks if there is wall the player is touching
    bool CheckForNewWallHit()
    {
        if (PlayerFrontChecker.isTouchingFront)
        {
            SetWallJumpingToFalse();
            return true;
        }
        else
        {
            return false;
        }
    }

    //check conditions for wall sliding and handle it as well as wall jumping
    void WallSlideCheck()
    {
        //set wallslide to true if parameters are met and vice versa
        if (PlayerFrontChecker.isTouchingFront == true)
        {
            wallSliding = true;
        }
        else if (PlayerFrontChecker.isTouchingFront == false)
        {
            wallSliding = false;
        }

        //if player is wall sliding, apply relevant velocity change
        if (wallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            UnityEngine.Debug.Log(rb.velocity.y);
        }

        //if player presses key while wall sliding, set wall jumping to true and set it to false after invoke time
        if (Input.GetAxisRaw("Jump") != 0 && wallSliding == true)
        {
            //check which direction the walljump should be in
            if (facingRight == true)
            {
                storedMoveInput = -1;
            }
            else if (facingRight == false)
            {
                storedMoveInput = 1;
            }

            //flip the player before the next movement
            Flip();

            Audio.PlayOneShot(Jump);

            //start inverting input, this is meant to allow for the player to do consecutive wall jumps while holding one directional input
            //if the player changes direction, input will stop being inverted
            invertInput = !invertInput;

            //wall jumping removes extra jumps, this can be changed if desired
            jumps = 0;

            //set walljumping to true so we can apply the walljumping movement
            wallJumping = true;

            //while the player is in a wall jump chain, reset the timer
            wallJumpTime = wallJumpTimeMax;
        }

        //if wall jumping, apply the relevant movement
        if (wallJumping)
        {
            //stop inverting input if the player changes input directions while wall jumping
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                invertInput = false;
            }

            rb.velocity = new Vector2(xWallForce * storedMoveInput, yWallForce);
        }

        //check if we're wallsliding still after the wall jump for jump chains
        if (PlayerFrontChecker.isTouchingFront == true)
        {
            wallSliding = true;
        }
        else if (PlayerFrontChecker.isTouchingFront == false)
        {
            wallSliding = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= (-1);
        transform.localScale = Scaler;
    }

    void WallJumpTimer()
    {
        if (wallJumpTime <= 0)
        {
            wallJumping = false;
            wallJumpTime = wallJumpTimeMax;
        }

        wallJumpTime -= Time.deltaTime;
    }
}
