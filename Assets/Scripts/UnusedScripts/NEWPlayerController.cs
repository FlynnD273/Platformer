//////////////////
//By: Dev Dhawan
//Date: 12/14/2020
//Description: Player controller for 2D platformer.
//////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWPlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;

    private Rigidbody2D myRB;
    private Animator myAnim;

    private CapsuleCollider2D myCC;

    private bool facingRight = true;

    private bool isGrounded;

    private int extraJumps;
    public int maxJumps = 2;
    //[Tooltip("Must be at a minimum 1")]

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        myCC = GetComponent<CapsuleCollider2D>();
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isGrounded == true)
        {
            extraJumps = maxJumps;
        }
        if (/*(Input.GetKeyDown(KeyCode.Space) && isGrounded) ||*/ (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded))
        {
            myRB.velocity = Vector2.up * jumpForce;
        }
        else if (/*(Input.GetKeyDown(KeyCode.Space) && extraJumps > 1) ||*/ (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 1))
        {
            myRB.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        myAnim.SetBool("Grounded", true);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        isGrounded = true;
        myAnim.SetBool("Grounded", true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
        myAnim.SetBool("Grounded", false);
    }

    void FixedUpdate()
    { 
        moveInput = Input.GetAxisRaw("Horizontal");
        myRB.velocity = new Vector2(moveInput * speed, myRB.velocity.y);

        /*if (moveInput == 0)
        {
            myAnim.SetBool("Walking", false);
        }
        else
        {
            myAnim.SetBool("Walking", true);
        }*/

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180, 0f);
    }
}
