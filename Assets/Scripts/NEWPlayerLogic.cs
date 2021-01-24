//////////////////
//By: Dev Dhawan and Thomas Allen
//Date: 1/15/2020
//Description: Player Logic fpr 2D game.
//////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NEWPlayerLogic : MonoBehaviour
{
    //respawn when player collides with something
    private Vector3 respawnPos;

    public Color checkActive = new Color(1, 0, 1, 1);
    public Color checkInactive = new Color(1, 1, 1, 1);
    public Color playerHit = new Color(0, 1, 1, 1);

    //Player health
    [Header("Health")]
    public float maxHealth = 100;
    public static float health = 0;
    public int lives = 5;
    private float temp;
    //Player Energy
    [Header("Energy")]
    public float maxEnergy = 60;
    public static float energy;
    public float waitforRegen = 20;
    //Melee
    [Header("Melee")]
    public GameObject Sword;
    public float WaitForMelee = 1;

    [Header("Energy")]
    //Energy Timer variables
    [SerializeField] private float regenCounter;
    public static bool startRegen = false;
    //Melee Timer variables
    //private float meleeCounter;
    private bool inMeleeFrame = false;
    private bool canUseMelee = true;

    //Classes being initialized
    private GameObject currentCheckPoint;
    private ProjectileHandler projectile;
    private FBProjectileMotion FireBall;
    private GameManager gameManager;
    private Animator playerAnim;

    public Rigidbody2D MyRB;

    public AudioClip PlayerHurtSound; //sound for when player is hurt or takes damage
    public static bool hasKey;
    public AudioClip keySound;
    public GameObject key;
    public  float DamgeCooldown;

    // Start is called before the first frame update
    void Start()
    {
        //health
        health = maxHealth;
        //energy
        energy = maxEnergy;
        //respawn point
        respawnPos = transform.position;
        //set object class
        projectile = FindObjectOfType<ProjectileHandler>();
        FireBall = FindObjectOfType<FBProjectileMotion>();
        gameManager = FindObjectOfType<GameManager>();
        //timer
        regenCounter = waitforRegen;
        //meleeCounter = WaitForMelee;
        playerAnim = GetComponent<Animator>();
        

    }

    //Checking for all collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ALL projectile collisions
        if (collision.gameObject.CompareTag("Kunai"))
        {
            projectile.IncreaseKun(1);
        }
        if (collision.gameObject.CompareTag("Shuriken"))
        {
            projectile.IncreaseSha(1);
        }
        if (collision.gameObject.CompareTag("FireballEnemy"))
        {
            Subhealth(40);
            StartCoroutine(ChangePlayerColor());
        }
        if (collision.gameObject.CompareTag("KunaiEnemy"))
        {
            Subhealth(30);
            StartCoroutine(ChangePlayerColor());
        }
        if (collision.gameObject.CompareTag("ShurikenEnemy"))
        {
            Subhealth(10);
            StartCoroutine(ChangePlayerColor());
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Subhealth(20);
            StartCoroutine(ChangePlayerColor());
            
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        //checking for tilemap
        //if (collision.gameObject.CompareTag("Moving"))
        //{
        //    transform.SetParent(null);
        //}
    }

    //Looking for Level Door
    private void OnTriggerStay2D(Collider2D collision)
    {
        LevelDoor LD = collision.GetComponent<LevelDoor>();
        if (LD != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene(LD.NextLevel);
            }
        }
    }

    //Collision with CheckPoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CheckPoint"))
        {
            //set a new position for the player to respawn
            respawnPos = collision.transform.position;
            //set old location color back to inactive
            if (currentCheckPoint != null)
            {
                currentCheckPoint.GetComponent<SpriteRenderer>().color = checkInactive;
            }
            //Set current check point to active color
            currentCheckPoint = collision.gameObject;
            if (currentCheckPoint != null)
            {
                currentCheckPoint.GetComponent<SpriteRenderer>().color = checkActive;
            }
        }
        if (collision.gameObject.CompareTag("Health"))
        {
            temp = health + 25;
            health = temp;
            if (health <= maxHealth)
            {
                health = maxHealth;
            }
        }
        //For deathobjects
        if (collision.gameObject.CompareTag("Death"))
        {
            //Reset Player
            //only subtract lives once, even if multiple collisions occur. (From Cooper)
            //since we move the player to their respawn once, we subtract lives once. Therefore, any multiple life losses don't happen.
            if (transform.position != respawnPos)
            {
                lives--;
            }
            transform.position = respawnPos;
            
            health = maxHealth;
        }
        //enable use of kunai and shuriken when picking up
        if (collision.gameObject.CompareTag("Kunai"))
        {
            Projectile.boolKunai = true;
        }
        if (collision.gameObject.CompareTag("Shuriken"))
        {
            Projectile.boolShuriken = true;
        }

        if (collision.gameObject.CompareTag("Key"))
        {
            hasKey = true;
            collision.gameObject.transform.parent = gameObject.transform;
            gameObject.GetComponent<AudioSource>().PlayOneShot(keySound);
        }
        if (collision.gameObject.CompareTag("Enemy") && DamgeCooldown <= 0)
        {
            DamgeCooldown = 4;
            health -= 15;
        }

        if (collision.gameObject.CompareTag("DragonFire") && DamgeCooldown <= 0)
        {
            DamgeCooldown = 4;
            Subhealth(34);
            StartCoroutine(ChangePlayerColor());
        }
    }

    public void FBregen()
    {
        regenCounter = waitforRegen * 3;
    }

    //Change in energy function (Used when energy is being needed to change)
    public bool EnergyChange(float amount, int switchProj)
    {
        //Checking if using FireBall is possible
        if (energy >= 60 && switchProj == 4)
        {
            energy = 0;
            //healthBar.MoveEnergybar(amount, true);
            startRegen = true;
            return true;
        }
        //changing energy's value
        //float energyIntial = energy;
        temp = energy - amount;
        energy = temp;
        //Checking if firing projectile is possible
        if (energy >= 0)
        {
            //healthBar.MoveEnergybar(amount, true);
            if (!startRegen)
            {
                startRegen = true;
                return true;
            }
            //Happens when Regen Counter is active
            else
            {
                regenCounter = waitforRegen;
                return true;
            }
        }
        if (energy < 0)
        {
            energy = 0;
            //energy = energyIntial;
        }
        return false;
        //Returns false for no shooting returns true to shoot
    }

    //subtracting health
    void Subhealth(float amount)
    {
        health -= amount;
        gameObject.GetComponent<AudioSource>().PlayOneShot(PlayerHurtSound);
    }

    //Changes color on the event that player is hit by enemy weapon
    //Look at enemy projectile collisions for more details
    IEnumerator ChangePlayerColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = playerHit;
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<SpriteRenderer>().color = checkInactive;
    }

    // Update is called once per frame
    void Update()
    {
        DamgeCooldown -= Time.deltaTime;
        //Checks for respawn
        if (health <= 0)
        {
            lives--;
            transform.position = respawnPos;
            health = maxHealth;
        }
        if(Input.GetMouseButtonDown(1) && canUseMelee)
        {
            if (EnergyChange(5, 1))
            {
                inMeleeFrame = true;
            }
        }
        //When lives = 0 
        if (lives <= 0)
        {
            SceneManager.LoadScene("LoseScene");
            energy = maxEnergy;
            health = maxHealth;
            lives = 5;
        }
        //energy Timer starting
        if (startRegen)
        {
            regenCounter -= Time.deltaTime;
            if (regenCounter <= 0)
            {
                float inc = (Time.deltaTime * 5);
                temp = temp + inc;
                energy = temp;
                //Stop Timer once energy reaches its limit       
                if (energy >= maxEnergy)
                {
                    startRegen = false;
                    regenCounter = waitforRegen;
                }
            }
        }
        //Melee timer starting
        if (inMeleeFrame)
        {
            if (canUseMelee)
            {
                canUseMelee = false;
                Sword.SetActive(true);
                playerAnim.SetTrigger("Sword");
                Sword.GetComponent<Animator>().SetTrigger("Sword");
            }
            else if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("SwordAttack"))
            {
                Sword.SetActive(false);
                inMeleeFrame = false;
                canUseMelee = true;
                //meleeCounter = WaitForMelee;
            }
            //meleeCounter -= Time.deltaTime;
        }
        if (hasKey)
        {
            key.transform.localPosition = new Vector3(0, 1, 1);

        }
    }
}