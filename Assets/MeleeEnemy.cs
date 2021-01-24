using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MeleeEnemy : MonoBehaviour
{

    public GameObject LeftWaypoint;
    public GameObject RightWaypoint;

    public GameObject player;

    [SerializeField] Transform playerTransform;
    [SerializeField] Animator enemyAnim;
    [SerializeField] Rigidbody2D myRB;
    SpriteRenderer enemySR;


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

    private RaycastHit2D hit;

    public LayerMask raycastMask;
    public float rayCastLength;
    public Transform rayCast;
    public float distance;
    private Transform target;

    [SerializeField] bool hurting = false;

    [SerializeField] CapsuleCollider2D AttackCollider;
    [SerializeField] float timer;


    public GameObject drop1;
    public GameObject drop2;
    public int maxDrops;
    public float spawnNumber;

    void Start()
    {
        //get the player transform   
        playerTransform = player.GetComponent<Transform>();
        //enemy animation and sprite renderer 
        enemyAnim = gameObject.GetComponent<Animator>();
        enemySR = GetComponent<SpriteRenderer>();
        myRB = GetComponent<Rigidbody2D>();

        AttackCollider.enabled = false;

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
    }

    private void Death()
    {
        AttackCollider.enabled = false;
        enemyAnim.SetBool(walking, false);
        enemyAnim.SetBool(hurt, false);
        enemyAnim.SetBool(attack, false);

        hurting = true;

        enemyAnim.SetBool(death, true);

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

        Invoke("DestroyEnemy", 1f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void ResetTimer()
    {
        timer = 0;
    }

    private void Idle()
    {
        
        AttackCollider.enabled = false;
        enemyAnim.SetBool(walking, false);
        enemyAnim.SetBool(hurt, false);
        enemyAnim.SetBool(attack, false);

        hurting = false;
    }

    private void Attack()
    {
        
        AttackCollider.enabled = true;

        enemyAnim.SetBool(walking, false);
        
        enemyAnim.SetBool(attack, true);
        
    }

    void Walk()
    {
        if (dirRight)
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        else
            transform.Translate(-Vector2.right * moveSpeed * Time.deltaTime);

        AttackCollider.enabled = false;
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
        hitPoints -= damage;
        enemyAnim.SetBool(attack, false);
        enemyAnim.SetBool(walking, false);
        enemyAnim.SetBool(hurt, true);

        hurting = true;

        Invoke("Idle", 0.5f);
    }

    private void resetAnim()
    {
        Idle();
    }
}