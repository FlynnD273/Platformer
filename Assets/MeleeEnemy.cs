using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MeleeEnemy : EnemyHandler
{

    //variables
    public int _moveSpeed;
    public int _attackDamage;
    public int _lifePoints;
    public float _attackRadius;


    public GameObject player;
    //movement
    public float _followRadius;
    //end
    [SerializeField] Transform playerTransform;
    [SerializeField] Animator enemyAnim;
    SpriteRenderer enemySR;

    void Start()
    {
        //get the player transform   
        playerTransform = player.GetComponent<Transform>();
        //enemy animation and sprite renderer 
        enemyAnim = gameObject.GetComponent<Animator>();
        enemySR = GetComponent<SpriteRenderer>();
        //set the variables
        setMoveSpeed(_moveSpeed);
        setAttackDamage(_attackDamage);
        setLifePoints(_lifePoints);
        setAttackRadius(_attackRadius);
        setFollowRadius(_followRadius);
    }

    // Update is called once per frame
    void Update()
    {
        if (checkFollowRadius(playerTransform.position.x, transform.position.x))
        {
            //if player in front of the enemies
            if (playerTransform.position.x < transform.position.x)
            {

                if (checkAttackRadius(playerTransform.position.x, transform.position.x))
                {
                    //for attack animation
                    enemyAnim.SetBool("AttackA", true);
                }
                else
                {
                    this.transform.position += new Vector3(-getMoveSpeed() * Time.deltaTime, 0f, 0f);
                    //for attack animation
                    enemyAnim.SetBool("AttackA", false);
                    //walk
                    enemyAnim.SetBool("Walking", true);
                    enemySR.flipX = true;
                }

            }
            //if player is behind enemies
            else if (playerTransform.position.x > transform.position.x)
            {
                if (checkAttackRadius(playerTransform.position.x, transform.position.x))
                {
                    //for attack animation
                    enemyAnim.SetBool("AttackA", true);
                }
                else
                {
                    this.transform.position += new Vector3(getMoveSpeed() * Time.deltaTime, 0f, 0f);
                    //for attack animation
                    enemyAnim.SetBool("AttackA", false);
                    //walk
                    enemyAnim.SetBool("Walking", true);
                    enemySR.flipX = false;
                }


            }
        }
        else
        {
            enemyAnim.SetBool("Walking", false);
        }


    }
}