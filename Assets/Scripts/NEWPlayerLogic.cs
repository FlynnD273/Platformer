//////////////////
//By: Dev Dhawan
//Date: 12/17/2020
//Description: Player Logic fpr 2D game.
//////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NEWPlayerLogic : MonoBehaviour
{
    //respawn when player collides with something
    private Vector3 respawnPos;

    public Color checkActive = new Color(1, 0, 1, 1);
    public Color checkInactive = new Color(1, 1, 1, 1);
    public Color playerHit = new Color(0, 1, 1, 1);

    //Player health
    public float healthPoints = 100;
    private float health = 0;
    public int lives = 5;
    private float temp;
    //Player Energy
    public float energyPoints = 60;
    private float energy;

    public float waitforRegen = 20;
    private float regenCounter;
    private bool startRegen;


    private GameObject currentCheckPoint;
    private Projectile projectile;
    private NEWFollowingCamera healthBar;

    public Rigidbody2D myRB;

    // Start is called before the first frame update
    void Start()
    {
        //health
        health = healthPoints;
        //energy
        energy = energyPoints;
        //respawn point
        respawnPos = transform.position;
        //set object class
        projectile = FindObjectOfType<Projectile>();
        healthBar = FindObjectOfType<NEWFollowingCamera>();
        //timer
        regenCounter = waitforRegen;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
        {
            transform.position = respawnPos;
            lives--;
            health = healthPoints;
        }
        else if (collision.gameObject.CompareTag("Moving"))
        {
            transform.SetParent(collision.transform);
        }
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
            healthBar.MoveHealthbar(30, true);
            StartCoroutine(ChangePlayerColor());
        }
        if (collision.gameObject.CompareTag("shurikanEnemy"))
        {
            Subhealth(10);
            healthBar.MoveHealthbar(10, true);
            StartCoroutine(ChangePlayerColor());
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Subhealth(20);
            healthBar.MoveHealthbar(20, true);
            StartCoroutine(ChangePlayerColor());
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Moving"))
        {
            transform.SetParent(null);
        }
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
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
    }

    public bool EnergyChange(bool decORIncEne, float amount)
    {
        if (decORIncEne == true)
        {
            temp = energy - amount;
        }
        if (decORIncEne == false)
        {
            temp = energy + amount;
        }
        energy = temp;
        Debug.Log(energy);
        if (energy >= 0)
        {
            healthBar.MoveEnergybar(amount, true);
            if (startRegen == false)
            {
                startRegen = true;
            }
            if (startRegen == true)
            {
                regenCounter = waitforRegen;
            }
            return true;
        }
        else
        {
            energy = 0;
            return false;
        }
    }
    void Subhealth(float amount)
    {
        temp = health - amount;
        health = temp;
    }

    IEnumerator ChangePlayerColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = playerHit;
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<SpriteRenderer>().color = checkInactive;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            lives--;
            transform.position = respawnPos;
            health = healthPoints;
            healthBar.MoveHealthbar(healthPoints, false);
        }
        if (energy <= 0)
        {
            startRegen = true;
        }
        if (lives <= 0)
        {
            energy = energyPoints;
            health = healthPoints;
        }
        if (startRegen == true)
        {
            regenCounter -= Time.deltaTime;
            Debug.Log("aa" + regenCounter);
            if (regenCounter <= 0)
            {
                float inc = (Time.deltaTime * 3);
                temp = temp + inc;
                Debug.Log("temp" + temp);
                energy = temp;
                healthBar.MoveEnergybar(inc, false);
                Debug.Log("ener" + energy);
                
                if (energy >= energyPoints)
                {
                    startRegen = false;
                    regenCounter = waitforRegen;
                }
            }
        }
    }
}