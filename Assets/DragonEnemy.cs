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


    public GameObject LeftWaypoint;
    public GameObject RightWaypoint;

    public GameObject player;

    [SerializeField] Animator enemyAnim;


    [SerializeField] public int moveSpeed;
    [SerializeField] public int attackDamage;
    [SerializeField] public int hitPoints;
    [SerializeField] public float attackDistance;
    [SerializeField] public float attackCooldown;

    public bool dirRight = true;

    public string EnemyName;

    private string idle;
    private string walking;
    private string attack;
    private string death;
    private string hurt;

    private Vector3 waypoint1;
    private Vector3 waypoint2;

    public Vector2 Distance;
    private RaycastHit2D hit;

    public LayerMask raycastMask;
    public float rayCastLength;
    public Transform rayCast;
    public float distance;

    [SerializeField] bool hurting = false;


    [SerializeField] public GameObject fireblast;
    [SerializeField] float timer;


    public GameObject drop1;
    public GameObject drop2;
    public int maxDrops;
    public float spawnNumber;


    
    private bool dead;

    public AudioClip EnemyHurt;
    public AudioClip EnemyAttack;
    private bool hasAttacked;

    void Start()
    {
        
        //enemy animation
        enemyAnim = gameObject.GetComponent<Animator>();


        fireblast.SetActive(false);

        idle = EnemyName + "IsIdle";
        walking = EnemyName + "IsWalking";
        attack = EnemyName + "AttackTriggered";
        death = EnemyName + "DeathTriggered";
        hurt = EnemyName + "IsHurt";

        waypoint1 = LeftWaypoint.transform.position;
        waypoint2 = RightWaypoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (transform.position.x >= waypoint2.x)
        {
            dirRight = false;
            Flip();
        }

        if (transform.position.x <= waypoint1.x)
        {
            dirRight = true;
            Flip();
        }

        Distance = transform.position - player.transform.position;

        hit = Physics2D.Raycast(rayCast.position, transform.right, rayCastLength, raycastMask);

        if (hit.collider != null)
        {
            distance = Vector2.Distance(transform.position, player.transform.position);
        }

        if (!hurting)
        {
            if (timer < attackCooldown)
            {
                Idle();
            }

            else if (distance <= attackDistance)
            {
                if (Mathf.Sign(Distance.x) == Mathf.Sign(transform.localScale.x))
                {
                    Flip();
                }
                if (!hasAttacked)
                    gameObject.GetComponent<AudioSource>().PlayOneShot(EnemyAttack);
                Attack();
                Invoke("ResetTimer", 1f);
            }
            else if (distance <= attackDistance)
                Idle();
            else
                Walk();
        }

        if (hitPoints <= 0)
        {
            Death();
        }

        if (hurting)
        {
            FireballOff();
        }
    }

    private void Death()
    {
        fireblast.SetActive(false);
        enemyAnim.SetBool(walking, false);
        enemyAnim.SetBool(hurt, false);
        enemyAnim.SetBool(attack, false);



        enemyAnim.SetBool(death, true);

        hurting = true;

        if (!dead)
        {
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
        dead = true;
        Invoke("DestroyEnemy", 1f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void ResetTimer()
    {
        timer = 0;
        hasAttacked = false;
    }

    private void Idle()
    {

        fireblast.SetActive(false);
        enemyAnim.SetBool(walking, false);
        enemyAnim.SetBool(hurt, false);
        enemyAnim.SetBool(attack, false);
        hurting = false;
    }

    private void Attack()
    {
       
        Invoke("FireballOn", 0.5f);

        enemyAnim.SetBool(walking, false);

        enemyAnim.SetBool(attack, true);

        Invoke("FireballOff", 4.5f);

        hasAttacked = true;
    }

    private void FireballOn()
    {
        if (!hurting)
            fireblast.SetActive(true);
    }

    private void FireballOff()
    {
        fireblast.SetActive(false);
    }

    void Walk()
    {
        if (dirRight)
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        else
            transform.Translate(-Vector2.right * moveSpeed * Time.deltaTime);

        fireblast.SetActive(false);
        enemyAnim.SetBool(attack, false);
        enemyAnim.SetBool(walking, true);

    }

    private void Flip()
    {

        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void TakeDamage(int damage)
    {
        if (!hurting)
            gameObject.GetComponent<AudioSource>().PlayOneShot(EnemyHurt);
        hitPoints -= damage;
        enemyAnim.SetBool(attack, false);
        enemyAnim.SetBool(walking, false);
        enemyAnim.SetBool(hurt, true);

        hurting = true;

        Invoke("Idle", 1f);
    }

    private void resetAnim()
    {
        Idle();
    }
}