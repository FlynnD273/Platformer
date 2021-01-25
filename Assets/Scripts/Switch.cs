//////////////////
//By: Dev Dhawan
//Date: 1/18/2020
//Description: Switch script.
//////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject wall;
    public Color checkActive = new Color(1, 0, 1, 1);
    public Color checkInactive = new Color(1, 1, 1, 1);
    public bool doDestroyORCreate = true;
    private bool isActive = true;

    //Checks if collision with kunai happens
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword") || collision.gameObject.CompareTag("Kunai") || collision.gameObject.CompareTag("Shuriken"))
        {
            
            if (isActive == true)
            {
                gameObject.GetComponent<SpriteRenderer>().color = checkActive;
                isActive = false;
                if(doDestroyORCreate == true)
                {
                    doDestroyORCreate = false;
                }
                else
                {
                    doDestroyORCreate = true;
                }
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().color = checkInactive;
                isActive = true;
                if (doDestroyORCreate == true)
                {
                    doDestroyORCreate = false;
                }
                else
                {
                    doDestroyORCreate = true;
                }
            }
            if (collision.gameObject.CompareTag("Kunai") || collision.gameObject.CompareTag("Shuriken"))
            {
                Destroy(collision.gameObject);
            }
            if (doDestroyORCreate == true)
            {
                wall.SetActive(true);
            }
            if (doDestroyORCreate == false)
            {
                wall.SetActive(false);
            }
        }
    }
}
