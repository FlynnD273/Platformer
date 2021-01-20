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
    public GameObject sword;
    public float waitForMelee = 1;

    [Header("Energy")]
    //Energy Timer variables
    [SerializeField] private float regenCounter;
    private bool startRegen = false;
    //Melee Timer variables
    private float meleeCounter;
    private bool inMeleeFrame = false;
    private bool canUseMelee = true;

    //Classes being initialized
    private GameObject currentCheckPoint;
    private Projectile projectile;
    private FBProjectileMotion FireBall;
    private GameManager gameManager;

    public Rigidbody2D myRB;

    public AudioClip playerHurtSound; //sound for when player is hurt or takes damage


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
        projectile = FindObjectOfType<Projectile>();
        FireBall = FindObjectOfType<FBProjectileMotion>();
        gameManager = FindObjectOfType<GameManager>();
        //timer
        regenCounter = waitforRegen;
        meleeCounter = waitForMelee;

    }

    //Checking for all collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //For Lava
        if (collision.gameObject.CompareTag("Death"))
        {
            //Reset Player
            transform.position = respawnPos;
            lives--;
            health = maxHealth;
        }
        //For TileMap
        else if (collision.gameObject.CompareTag("Moving"))
        {
            transform.SetParent(collision.transform);
        }
        //ALL projectile collisions
        if (collision.gameObject.CompareTag("kunai"))
        {
            projectile.IncreaseKun(1);
        }
        if (collision.gameObject.CompareTag("shurikan"))
        {
            projectile.IncreaseSha(1);
        }
        if (collision.gameObject.CompareTag("kunaiEnemy"))
        {
            Subhealth(30);
            //healthBar.MoveHealthbar(30, true);
            StartCoroutine(ChangePlayerColor());
        }
        if (collision.gameObject.CompareTag("shurikenEnemy"))
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
        if (collision.gameObject.CompareTag("Moving"))
        {
            transform.SetParent(null);
        }
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
    }

    //Change in energy function (Used when energy is being needed to change)
    public bool EnergyChange(bool decORIncEne, float amount, int switchProj)
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
        float energyIntial = energy;
        if (decORIncEne == true)
        {
            temp = energy - amount;
        }
        if (decORIncEne == false)
        {
            temp = energy + amount;
        }
        energy = temp;
        //Checking if firing projectile is possible
        if (energy >= 0)
        {
            //healthBar.MoveEnergybar(amount, true);
            if (startRegen == false)
            {
                Debug.Log("set Timer " + energy);
                startRegen = true;
            }
            //Happens when Regen Counter is active
            if (startRegen == true)
            {
                Debug.Log("restart timer " + energy);
                regenCounter = waitforRegen;
                return true;
            }
            return true;
        }
        if (energy < 0)
        {
            energy = 0;
            Debug.Log("no energy " + energy);
            //energy = energyIntial;
        }
        return false;
        //Returns false for no shooting returns true to shoot
    }

    //subtracting health
    void Subhealth(float amount)
    {
        temp = health - amount;
        health = temp;
        gameObject.GetComponent<AudioSource>().PlayOneShot(playerHurtSound);

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
        Debug.Log(lives);
        //Checks for respawn
        if (health <= 0)
        {
            lives--;
            transform.position = respawnPos;
            health = maxHealth;
        }
        //for starting the timer
        if (energy <= 0)
        {
            startRegen = true;
        }
        if(Input.GetMouseButtonDown(1) && canUseMelee == true)
        {
            if (EnergyChange(true, 5, 1) == true)
            {
                inMeleeFrame = true;
            }
        }
        //When lives = 0 
        if (lives <= 0)
        {
            SceneManager.LoadScene(2);
            energy = maxEnergy;
            health = maxHealth;
            lives = 5;
        }
        //energy Timer starting
        if (startRegen == true)
        {
            regenCounter -= Time.deltaTime;
            if (regenCounter <= 0)
            {
                float inc = (Time.deltaTime * 20);
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
        if (inMeleeFrame == true)
        {
            canUseMelee = false;
            meleeCounter -= Time.deltaTime;
            if(meleeCounter > 0)
            {
                sword.SetActive(true);
            }
            else
            {
                sword.SetActive(false);
                inMeleeFrame = false;
                canUseMelee = true;
                meleeCounter = waitForMelee;
            }
        }
    }
}