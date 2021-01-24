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
    private RangedEnemy rangedEnemy;
    private DragonEnemy dragonEnemy;

    private void Start()
    {
        swordAnimator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //if the projectile hits an enemy, deal damage to it and despawn projectile
        MeleeEnemy myEnemy = collision.GetComponent<MeleeEnemy>();
        if (myEnemy != null)
        {
            myEnemy.TakeDamage(damage);
        }
        if (myEnemy == null)
            rangedEnemy = collision.GetComponent<RangedEnemy>();
        if (rangedEnemy != null)
        {
            rangedEnemy.TakeDamage(damage);
            
        }
        if (rangedEnemy == null && rangedEnemy == null)
            dragonEnemy = collision.GetComponent<DragonEnemy>();
        if (dragonEnemy != null)
        {
            dragonEnemy.TakeDamage(damage);
            
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
