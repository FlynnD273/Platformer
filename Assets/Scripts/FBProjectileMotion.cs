//////////////////
//By: Dev Dhawan
//Date: 1/11/2020
//Description: The motion of Projectile FireBall.
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

    //mini bool
    public bool boolMini = false;
    public bool isExplosion = false;

    //Death Time
    public float deathTime = 8.0f;

    //declare components
    public Rigidbody2D myRB;
    private Projectile projectile;
    //explosion when gameobject death
    public GameObject explosion;
    
    
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
        if (collision.gameObject.CompareTag("kunaiEnemy") || collision.gameObject.CompareTag("shurikenEnemy"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(boolMini == true)
            {
                Destroy(gameObject);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        deathTime -= Time.deltaTime;
        if (boolRotate == true)
        {
            transform.Rotate(0, 0, rotationSpeed);
        }

        if (deathTime == 0 && boolMini == false && isExplosion == false)
        {
            Debug.Log("ha");
            Vector2 spawn = transform.position;
            spawn.y = transform.position.y + 10;
            transform.position = spawn;
            //Instantiate(explosion, transform.position, transform.rotation);
        }

        Destroy(gameObject, deathTime);
    }
}
