/////////////////////
///Name: Thomas Allen
///Date: 1/24/21
///Desc: Add thsis to a dragon enemy to allow it to move between two points and attack the player
///Thanks to Flynn for making the flipping work properly
////////////////////



using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PolygonCollider2D))]

public class DragonEnemy : MonoBehaviour
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
    [SerializeField] public float attackCooldown; //how long enemy has to wait in between attacks

    [Header("Weapons")]
    [SerializeField] public GameObject fireblast;

    [Header("Weapons to Drop on Death")]
    //weapons to drop
    public GameObject drop1;
    public GameObject drop2;
    public int maxDrops; //max possible number to drop

    [Header("Sound Effects")]
    public AudioClip EnemyHurt; //plays when takeDamge function is called
    public AudioClip EnemyAttack; //plays when enemy attacks
    public AudioClip EnemyDeath; //plays when enemy dies

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

    void Start()
    {
        // get the animator component
        enemyAnim = gameObject.GetComponent<Animator>();
        //diable fire attack
        fireblast.SetActive(false);

        //set animator bool strings depending on the name of the enemy
        idle = EnemyName + "IsIdle";
        walking = EnemyName + "IsWalking";
        attack = EnemyName + "AttackTriggered";
        death = EnemyName + "DeathTriggered";
        hurt = EnemyName + "IsHurt";
        //set waypoints to the startng positions of the gameobject waypoints
        waypoint1 = LeftWaypoint.transform.position;
        waypoint2 = RightWaypoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //update timer
        timer += Time.deltaTime;

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
        distance = Vector2.Distance(transform.position, player.transform.position);

        //take action is enemy is not hurting
        if (!hurting)
        {
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
            //move if cannot take other actions
            else
                Walk();
        }

        //die if health hits 0
        if (hitPoints <= 0)
        {
            Death();
        }
        //turn of flame if draogn is damaged
        if (hurting)
        {
            FireballOff();
        }
    }

    private void Death()
    {
        //dable attacks
        fireblast.SetActive(false);
        //disable all other animations
        enemyAnim.SetBool(walking, false);
        enemyAnim.SetBool(hurt, false);
        enemyAnim.SetBool(attack, false);
        //start death animation
        enemyAnim.SetBool(death, true);

        //hurt enemy
        hurting = true;

        //make sure they ren't already dead
        if (!dead)
        {
            //spawn drops
            gameObject.GetComponent<AudioSource>().PlayOneShot(EnemyDeath);
            print("spawned");
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
        //enemy becomes dead
        dead = true;
        //destory enemy after animation is done
        Invoke("DestroyEnemy", 1f);
    }
    private void DestroyEnemy()
    {
        //destroy enemy
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
        fireblast.SetActive(false);
        enemyAnim.SetBool(walking, false);
        enemyAnim.SetBool(hurt, false);
        enemyAnim.SetBool(attack, false);
        //"heal" hurt state
        hurting = false;
    }

    private void Attack()
    {
       //turn on flame after animation windup
        Invoke("FireballOn", 0.5f);
        //reset bools
        enemyAnim.SetBool(walking, false);
        //start attack animation
        enemyAnim.SetBool(attack, true);
        //turn off flame after duration
        Invoke("FireballOff", 4.5f);
        //update bool
        hasAttacked = true;
    }

    private void FireballOn()
    {
        //turn on if not hurting
        if (!hurting)
            fireblast.SetActive(true);
    }

    private void FireballOff()
    {
        //turn off attack
        fireblast.SetActive(false);
    }

    //movement state
    void Walk()
    {
        //translate to next waypoint
        Vector3 v = new Vector3(Mathf.Sign(transform.localScale.x), 0, 0);
        transform.Translate(v * moveSpeed * Time.deltaTime);

        //diable attacks
        fireblast.SetActive(false);
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
        //reset animations
        enemyAnim.SetBool(attack, false);
        enemyAnim.SetBool(walking, false);
        //play hurt animation
        enemyAnim.SetBool(hurt, true);
        //update bools
        hurting = true;
        //reset to default state
        Invoke("Idle", 0.5f);
    }
}