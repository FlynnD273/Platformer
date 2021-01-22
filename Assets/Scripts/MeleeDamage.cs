//////////////////
//By: Dev Dhawan
//Date: 1/18/2020
//Description: Melee script.
//////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    public static Animator swordAnimator;
    public int damage = 15;

    private void Start()
    {
        swordAnimator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //if the projectile hits an enemy, deal damage to it and despawn projectile
        Enemy myEnemy = collision.GetComponent<Enemy>();
        if (myEnemy != null)
        {
            myEnemy.TakeDamage(damage);
        }
        CrateDrops crateDrops;
        crateDrops = collision.GetComponent<CrateDrops>();
        if (crateDrops != null)
        {
            crateDrops.TakeDamage(damage);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("KunaiEnemy") || collision.gameObject.CompareTag("ShurikenEnemy"))
        {
            Destroy(collision.gameObject);
        }
    }
}
