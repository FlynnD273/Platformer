﻿//////////////////
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
    public bool ignoreTileMap = false;
    public float rotationSpeed = -5f;

    //Death Time
    public float deathTime = 8.0f;

    //Strength Variable
    public bool strength = false;

    //declare components
    public Rigidbody2D myRB;
    private Projectile projectile;

    [SerializeField] CrateDrops crateDrops;
    [SerializeField] MeleeEnemy meleeEnemy;
    [SerializeField] RangedEnemy rangedEnemy;
    [SerializeField] DragonEnemy dragonEnemy;
    [SerializeField] BossEnemy bossEnemy;

    // Start is called before the first frame update
    void Start()
    {
        //set the projectile speed when the projectile is spawned
        myRB.velocity = transform.right * speed;
        projectile = FindObjectOfType<Projectile>();
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        if (!isEnemyWeapon)
        {
            //if the projectile hits an enemy, deal damage to it and despawn projectile
            meleeEnemy = collision.GetComponent<MeleeEnemy>();

            if (meleeEnemy != null)
            {
                meleeEnemy.TakeDamage(damage);
                Destroy(gameObject);
            }
            if (meleeEnemy == null)
                rangedEnemy = collision.GetComponent<RangedEnemy>();
            if (rangedEnemy != null)
            {
                rangedEnemy.TakeDamage(damage);
                Destroy(gameObject);
            }
            if (rangedEnemy == null && rangedEnemy == null)
                dragonEnemy = collision.GetComponent<DragonEnemy>();
            if (dragonEnemy != null)
            {
                dragonEnemy.TakeDamage(damage);
                Destroy(gameObject);
            }
            if (dragonEnemy == null && rangedEnemy == null)
                bossEnemy = collision.GetComponent<BossEnemy>();
            if (bossEnemy != null)
            {
                bossEnemy.TakeDamage(damage);
                Destroy(gameObject);
            }
            crateDrops = collision.GetComponent<CrateDrops>();
            if (crateDrops != null)
            {
                crateDrops.TakeDamage(damage);
                Destroy(gameObject);
            }
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
            if (!isEnemyWeapon)
            {
                myRB.constraints = RigidbodyConstraints2D.FreezeAll;
                boolRotate = false;
                damage = 0;
            }
            if(isEnemyWeapon == true && ignoreTileMap == false)
            {
                boolRotate = false;
                damage = 0;
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.CompareTag("KunaiEnemy") || collision.gameObject.CompareTag("ShurikenEnemy") && ignoreTileMap == false)
        {
            if (strength)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
        if ((collision.gameObject.CompareTag("Kunai") || collision.gameObject.CompareTag("Shuriken")) && isEnemyWeapon && ignoreTileMap == false)
        {
            if (strength)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        if ((collision.gameObject.CompareTag("Kunai") || collision.gameObject.CompareTag("Shuriken")) && isEnemyWeapon && ignoreTileMap == true)
        {
                Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("FireBall") && ignoreTileMap == true &&  isEnemyWeapon == true)
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Sword"))
        {
            Destroy(collision.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (boolRotate)
        {
            transform.Rotate(0, 0, rotationSpeed);
        }
        Destroy(gameObject, deathTime);
    }
}
