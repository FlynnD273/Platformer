﻿/////////////////////
///Name: Thomas Allen
///Date: 1/24/21
///Desc: A boss with rnaged an melee states
///Thanks to Flynn for making the flipping work properly, adn thanks to Brakeys for the dissolve effect
////////////////////



using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class BossEnemy : MonoBehaviour
{
    [Header("Name of Enemy")]
    public string EnemyName;

    [Header("Movement Waypoints")]
    public GameObject LeftWaypoint; //farthest left enemy will patrol
    public GameObject RightWaypoint; //farthest right enemy will patrol

    [Header("Player")]
    public GameObject player; //the player in the scene

    [Header("Animator")]
    [SerializeField] Animator enemyAnim; //animator component of enemy


    [Header("Stats")]
    [SerializeField] public int moveSpeed; //speed enemy moves
    [SerializeField] public int hitPoints; //health
    [SerializeField] public float attackDistance; //range
    [SerializeField] public float rangedAttackDistance;
    [SerializeField] public float attackCooldown; //how long enemy has to wait in between attacks

    [Header("Weapons and Attack COlliders")]
    [SerializeField] GameObject projectile; //object to fire

    [Header("Weapons to Drop on Death")]
    [SerializeField] bool dropItemsOnDeath; //wther or not enemy drops items on death
    //weapons to drop
    public GameObject drop1;
    public GameObject drop2;
    public int maxDrops; //max possible number to drop

    [Header("Sound Effects")]
    public AudioClip EnemyHurt; //plays when takeDamge function is called
    public AudioClip EnemyAttack; //plays when enemy attacks
    public AudioClip EnemyDeath; //plays when enemy dies

    [Header("Boss Extras")]
    [SerializeField] Slider bossHealthbar;
    [SerializeField] GameObject bossHealthbarParent;

    
    private bool dirRight = true; //wether or not the enemy is moving right
    //strings for animator bools
    private string idle;
    private string walking;
    private string attack;
    private string death;
    private string hurt;

    private Vector3 waypoint1; //left waypoint
    private Vector3 waypoint2; //right waypoint
    private Vector2 Distance; //distance to player as a vector2
    private bool hurting = false; //wehter or not the enemy is in the "hurt" state
    private float timer; //countdown timer
    private float spawnNumber; //number of drops to spawn on death
    private bool hasAttacked; //wether or not enemy has atacked
    private bool dead; //wether or not enemy is dead

    private float distance; //distance as float
    private float offset; //offset to fire projectile
    private Vector3 difference; //differrence of positions
    private float rotz; //rotation to fire
    private int temp;
    private Quaternion originalPos; //oroginal position before firing

    Material material;

    bool isDissolving = false;
    float fade = 1f;

    void Start()
    {
        //get the animator component
        enemyAnim = gameObject.GetComponent<Animator>();

        //player
        player = GameObject.FindGameObjectWithTag("Player");
        //set animator bool strings depending on the name of the enemy
        idle = EnemyName + "IsIdle";
        walking = EnemyName + "IsWalking";
        attack = EnemyName + "AttackTriggered";
        death = EnemyName + "DeathTriggered";
        hurt = EnemyName + "IsHurt";
        //set waypoints to the startng positions of the gameobject waypoints
        waypoint1 = LeftWaypoint.transform.position;
        waypoint2 = RightWaypoint.transform.position;

        bossHealthbarParent.SetActive(false); //disabled by default

        // Get a reference to the material
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //die if health hits 0
        if (hitPoints <= 0)
        {
            Death();
        }
        //update timer
        timer += Time.deltaTime;
        bossHealthbar.value = hitPoints;

        //flip enemy when they pass a waypoint
        if (transform.position.x >= waypoint2.x)
        {
            Flip();
        }

        if (transform.position.x <= waypoint1.x)
        {
            Flip();
        }

        //calculate new distance to player
        Distance = transform.position - player.transform.position;

        //idle if timer is counting
        if (timer < attackCooldown)
        {
            Idle();
        }
        //if player is within attack range, attac them
        else if (Distance.sqrMagnitude <= attackDistance * attackDistance)
        {
            //flip to face the player
            if (Mathf.Sign(Distance.x) == Mathf.Sign(transform.localScale.x))
            {
                Flip();
            }
            //play attack sound
            if (!hasAttacked)
                gameObject.GetComponent<AudioSource>().PlayOneShot(EnemyAttack);
            //attack
            Attack();
            //reset cooldown
            Invoke("ResetTimer", 1f);
        }
        else if (Distance.sqrMagnitude <= rangedAttackDistance * rangedAttackDistance)
        {
            bossHealthbarParent.SetActive(true);
            //flip to face the player
            if (Mathf.Sign(Distance.x) == Mathf.Sign(transform.localScale.x))
            {
                Flip();
            }
            //play attack sound
            if (!hasAttacked)
                gameObject.GetComponent<AudioSource>().PlayOneShot(EnemyAttack);
            //attack
            RangedAttack();
            //reset cooldown
            Invoke("ResetTimer", 1f);
        }
        //move if cannot take other actions
        else
            Walk();

        if (isDissolving)
        {
            fade -= Time.deltaTime;

            if (fade <= 0f)
            {
                fade = 0f;
                isDissolving = false;
            }

            // Set the property
            material.SetFloat("_Fade", fade);
        }

    }


    private void RangedAttack()
    {


        //reset anim
        enemyAnim.SetBool(walking, false);
        //enable attack anim
        enemyAnim.SetBool(attack, true);
        //attack only once
        if (!hasAttacked)
        {
            //play attack sound
            gameObject.GetComponent<AudioSource>().PlayOneShot(EnemyAttack);
            //calculate trajectory of projectile
            difference = player.transform.position - transform.position;
            rotz = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotz + offset);
            bool facing = dirRight;
            //fire projectile
            fire();
            //check facing of enemy
            if (dirRight != facing)
            {
                Flip();
            }
            //reset position
            transform.rotation = originalPos;
        }
        //update bool
        hasAttacked = true;
    }

    //called to shoot projectile
    void fire()
    {
        //fire in diff directions depending on facing
        if (rotz >= 90 && rotz >= 0)
        {
            dirRight = false;
            Instantiate(projectile, transform.position, transform.rotation);
        }
        if (rotz <= 90 && rotz >= 0)
        {
            dirRight = false;
            Instantiate(projectile, transform.position, transform.rotation);
        }
        if (rotz <= -90 && rotz <= 0)
        {
            dirRight = true;
            Instantiate(projectile, transform.position, transform.rotation);
        }
        if (rotz >= -90 && rotz <= 0)
        {
            dirRight = true;
            Instantiate(projectile, transform.position, transform.rotation);
        }

    }


    //called when enemy has no health
    private void Death()
    {
        //enable dissolve effect
        isDissolving = true;
        //disable all other animations
        enemyAnim.SetBool(walking, false);
        enemyAnim.SetBool(attack, false);

        //hurt enemy
        hurting = true;

        //make sure they ren't already dead
        if (!dead)
            gameObject.GetComponent<AudioSource>().PlayOneShot(EnemyDeath);

        //dasable healthbar
        bossHealthbarParent.SetActive(false);
        //enemy becomes dead

        if (!dead)
            //destory enemy after animation is done
            Invoke("DestroyEnemy", 2f);
        dead = true;
    }

    private void DestroyEnemy()
    {
        if (dropItemsOnDeath)
        {
            //spawn drops
            spawnNumber = Random.Range(1, maxDrops);
            for (int i = 0; i < spawnNumber; i++)
            {
                Instantiate(drop1, transform.position, transform.rotation);
            }
            spawnNumber = Random.Range(1, maxDrops);
            for (int i = 0; i < spawnNumber; i++)
            {
                Instantiate(drop2, transform.position, transform.rotation);
            }
        }
        Destroy(gameObject);
    }

    private void ResetTimer()
    {
        //reset time and allow player to attack
        timer = 0;
        hasAttacked = false;
    }

    //default state
    private void Idle()
    {
        //reset attack collider and animations

        enemyAnim.SetBool(walking, false);
        enemyAnim.SetBool(attack, false);
        //"heal" hurt state
        hurting = false;
    }


    private void Attack()
    {
        //update bool
        hasAttacked = true;

        //disable walking
        enemyAnim.SetBool(walking, false);
        //start attack anim
        enemyAnim.SetBool(attack, true);

    }

    //movement state
    void Walk()
    {
        //translate to next waypoint
        Vector3 v = new Vector3(Mathf.Sign(transform.localScale.x), 0, 0);
        transform.Translate(v * moveSpeed * Time.deltaTime);


        enemyAnim.SetBool(attack, false);
        //enable walking anim
        enemyAnim.SetBool(walking, true);
    }

    //flip around
    private void Flip()
    {

        Vector3 temp = transform.localScale;
        temp.x *= -1;
        transform.localScale = temp;
    }

    //called to deduct health and play anim
    public void TakeDamage(int damage)
    {
        //play sound only once
        if (!hurting)
            gameObject.GetComponent<AudioSource>().PlayOneShot(EnemyHurt);
        //deduct health
        hitPoints -= damage;
        //update bools
        hurting = true;

    }

}