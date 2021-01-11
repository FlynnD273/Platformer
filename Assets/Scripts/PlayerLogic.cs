//Cooper Spring, 12/15/2020, game logic directly affecting the player
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLogic : MonoBehaviour
{
    private Vector3 respawn;

    public Color checkActive = new Color(1,0,1,1);

    public Color checkInactive = new Color(1,1,1,1);

    private GameObject currentCheckpoint;

    public static bool bottomCheckerDeathHit;

    // Start is called before the first frame update
    void Start()
    {
        respawn = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //respawn if player hits a death object
        if (collision.gameObject.CompareTag("Death"))
        {
            transform.position = respawn;
        }
        //move with moving platform if standing on it
        if (collision.gameObject.tag.Equals("Platform"))
        {
            this.transform.parent = collision.transform;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //reset player parent when it steps off platform
        if (collision.gameObject.tag.Equals("Platform"))
        {
            this.transform.parent = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            //set player location to new position
            respawn = collision.transform.position;

            //set the old checkpoint's color back to the inactive color
            if(currentCheckpoint != null)
            {
                currentCheckpoint.GetComponent<SpriteRenderer>().color = checkInactive;
            }

            //set current checkpoint to the latest hit checkpoint
            currentCheckpoint = collision.gameObject;

            //set the new checkpoint's color to the active color
            if (currentCheckpoint != null)
            {
                currentCheckpoint.GetComponent<SpriteRenderer>().color = checkActive;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //check if the bottom checker hits death tag object
        if(bottomCheckerDeathHit == true)
        {
            transform.position = respawn;
            //reset var
            bottomCheckerDeathHit = false;
            
        }
    }
}
