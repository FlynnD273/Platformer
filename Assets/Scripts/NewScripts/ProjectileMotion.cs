using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMotion : MonoBehaviour
{
    //speed and damage vars
    public float speed = 20f;
    public int damage = 40;
    //declare components
    public Rigidbody2D myRB;
    private Projectile projectile;
    
    // Start is called before the first frame update
    void Start()
    {
        //set the projectile speed when the projectile is spawned
        myRB.velocity = transform.right * speed;
        //set object class
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
            projectile.IncreaseKun(1);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Tilemap"))
        {
            //stop the projectile if it hits tiles
            myRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
