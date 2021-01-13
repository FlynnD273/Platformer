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
    public int healthPoints = 100;
    private int health = 0;
    public int lives = 5;
    private int temp;
    //Player Energy
    public int energyPoints = 101;
    private int energy;

    private GameObject currentCheckPoint;
    private Projectile projectile;

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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            temp = health - 20;
            health = temp;
            StartCoroutine(ChangePlayerColor());
            if (health <= 0)
            {
                lives--;
                transform.position = respawnPos;
                health = healthPoints;
            }
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

    public bool EnergyChange(bool decORIncEne, int amount)
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
        if (energy > 0)
        { 
            return true;
        }
        else
        {
            energy = 0;
            return false;
        }
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
        if (lives <= 0)
        {
            energy = energyPoints;
            health = healthPoints;
        }
    }
}