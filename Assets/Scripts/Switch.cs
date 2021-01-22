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
    public bool doDestroyORCreate = true;

    //Checks if collision with kunai happens
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword") || collision.gameObject.CompareTag("Kunai"))
        {
            if (doDestroyORCreate)
            {
                Destroy(wall);
            }
            if (!doDestroyORCreate)
            {
                wall.SetActive(true);
            }
            gameObject.GetComponent<SpriteRenderer>().color = checkActive;
            if (collision.gameObject.CompareTag("Kunai"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
