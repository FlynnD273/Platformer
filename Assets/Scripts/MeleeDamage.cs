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
    public int damage = 15;
    void OnTriggerEnter2D(Collider2D collision)
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
    }
}
