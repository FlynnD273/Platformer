//Cooper Spring, 12/15/2020, script that allows for bottomChecker collider to hold a static bool to be used by the player controller
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottomChecker : MonoBehaviour
{
    public static bool isTouchingBottom;


    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        //handle when player enters ground space
        if (!collision.isTrigger)
        {
            isTouchingBottom = true;
            //CooperPlayerController.myAnim.SetBool("Grounded", true);
        }

        //change pl var if player hits a death object
        if (collision.gameObject.CompareTag("Dangerous"))
        {
            PlayerLogic.bottomCheckerDeathHit = true;
        }
        
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        //maintain touching bottom state while grounded
        if (!collision.isTrigger)
        {
            isTouchingBottom = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
       
        //handle when the player leaves the ground
        if (!collision.isTrigger)
        {
            isTouchingBottom = false;
            //CooperPlayerController.myAnim.SetBool("Grounded", false);
        }
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            isTouchingBottom = false;
        }
        else
        {
            isTouchingBottom = true;
        }
        if (collision.gameObject.CompareTag("Dangerous"))
        {
            PlayerLogic.bottomCheckerDeathHit = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            isTouchingBottom = false;
        }
        else
        {
            isTouchingBottom = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            isTouchingBottom = false;
        }
        else
        {
            isTouchingBottom = true;
        }
    }
}
