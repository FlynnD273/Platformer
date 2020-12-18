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
    public int MaxJump = 2;
    public Vector2 StartPos { get; set; }

    public UnityEvent Die = new UnityEvent();

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float xScale;
    private bool prevJumped = true;
    private int jumps = 0;
    private Stopwatch FloatTime;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        xScale = transform.localScale.x;
        FloatTime = new Stopwatch();
        Die.AddListener(Reset);
        StartPos = rb.position;
        Player = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool onGround = false;
        Collider2D[] groundCollide = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(0.8f, 0.2f), 0);
        foreach (Collider2D c in groundCollide)
        {
            if (c.CompareTag("Ground"))
            {
                onGround = true;
                break;
            }
        }

        anim.SetBool("OnGround", onGround);

        bool jump = false;


        if (onGround)
        {
            jumps = MaxJump - 1;
        }
        
        if (Input.GetAxisRaw("Jump") > 0.5f)
        {
            if (jumps > 0 && !prevJumped)
            {
                if (!onGround)
                    jumps--;
                jump = true;
                FloatTime.Restart();
            }
            prevJumped = true;

            if (FloatTime.IsRunning && FloatTime.ElapsedMilliseconds < FloatDuration * 1000)
            {
                rb.AddForce(new Vector2(0, FloatForce));
            }
        }
        else
        {
            prevJumped = false;
            FloatTime.Stop();
        }

        float xSpeed = Input.GetAxisRaw("Horizontal") * WalkSpeed;

        rb.velocity = new Vector2(rb.velocity.x * XDecay, jump ? JumpSpeed : rb.velocity.y);
        rb.AddForce(new Vector2(xSpeed, 0));

        if (rb.velocity.y < 0)
        {
            FloatTime.Stop();
        }

        if (xSpeed != 0)
        {
            if (xSpeed < 0)
            {
                transform.localScale = new Vector3(-xScale, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
            }
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
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
}
