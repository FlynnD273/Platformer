//Cooper Spring, 1/14/2021, Script to control the buttons at the lose screen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CooperLoseLevelButtons : MonoBehaviour
{
    public bool Retry;
    public bool Exit;
    public bool Return;
    private void OnMouseUp()
    {
        //checks if a button was clicked with the start or quit select variable checked, 
        //which is passed because they have a trigger collider
        if (Retry)
        {
            //sends to a level
            Debug.Log(GameManager.Level);
            GameManager.Manager.RestartLevel();
        }

        if (Exit)
        {
            Application.Quit();
        }

        if (Return)
        {
            GameManager.Manager.SetLevel(0);
        }
    }
}
