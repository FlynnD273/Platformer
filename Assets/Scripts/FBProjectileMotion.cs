//////////////////
//By: Dev Dhawan
//Date: 1/11/2020
//Description: The motion of the Projectile.
//////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBProjectileMotion : MonoBehaviour
{
    //speed and damage vars
    public float speed = 2f;
    public int damage = 300;

    //rotation variables
    public bool boolRotate = false;
    public float rotationSpeed = -5f;

    //Death Time
    public float deathTime = 8.0f;

    //declare components
    public Rigidbody2D myRB;
    private Projectile projectile;

    
    
    // Start is called before the first frame update
    void Start()
    {
        //set the projectile speed when the projectile is spawned
        myRB.velocity = transform.right * speed;
        projectile = FindObjectOfType<Projectile>();
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        //if the projectile hits an enemy, deal damage to it and despawn projectile
        Enemy myEnemy = collision.GetComponent<Enemy>();
        if (myEnemy != null)
        {
            myEnemy.TakeDamage(damage);
        }
        if (collision.gameObject.CompareTag("kunaiEnemy") || collision.gameObject.CompareTag("shurikanEnemy"))
        {
            Destroy(collision.gameObject);
        }
    }

    
        

    // Update is called once per frame
    void Update()
    {
        if (boolRotate == true)
        {
            transform.Rotate(0, 0, rotationSpeed);
        }

        Destroy(gameObject, deathTime);
    }
}
