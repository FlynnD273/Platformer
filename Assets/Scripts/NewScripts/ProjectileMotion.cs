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
    public float speed = 20f;
    public int damage = 40;
    public bool playerKun = false;
    public Rigidbody2D myRB;
    private Projectile projectile;
    
    // Start is called before the first frame update
    void Start()
    {
        myRB.velocity = transform.right * speed;
        projectile = GameObject.FindObjectOfType<Projectile>();
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
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
            projectile.IncreaseKun(1);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Tilemap"))
        {
            myRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
