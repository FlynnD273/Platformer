//////////////////
//By: Dev Dhawan
//Date: 1/11/2020
//Description: The motion of the Projectile.
//////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotion : MonoBehaviour
{
    //speed and damage vars
    public float speed = 20f;
    public int damage = 40;

    //rotation variables
    public bool boolRotate = false;
    public bool isEnemyWeapon = false;
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
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //destroy kunai if it hits player
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Tilemap"))
        {
            //stop the projectile if it hits tiles
            if (isEnemyWeapon == false)
            {
                myRB.constraints = RigidbodyConstraints2D.FreezeAll;
                boolRotate = false;
                damage = 0;
            }
            else
            {
                boolRotate = false;
                damage = 0;
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.CompareTag("kunaiEnemy") || collision.gameObject.CompareTag("shurikenEnemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
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
