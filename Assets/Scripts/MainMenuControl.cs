//Cooper Spring, 11/5/2020, Script to control the main menu's buttons
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    //button bools
    public string Level;
    public bool startSelect;
    public bool quitSelect;
    public bool settingsSelect;
    private static bool inSettings = false;
    public GameObject MainMenuObject;
    public GameObject SettingsMenuObject;
    private void OnMouseUp()
    {
        //checks if a button was clicked with the start or quit select variable checked, 
        //which is passed because they have a trigger collider
        if (startSelect)
        {
            //sends to a level
            GameManager.Manager.NextLevel();
        }

        if (quitSelect)
        {
            Application.Quit();
        }

        if (settingsSelect)
        {
            //set the main menu to inactive so we can show the settings menu
            if (!inSettings)
            {
                MainMenuObject.SetActive(false);
                SettingsMenuObject.SetActive(true);
                inSettings = true;
            }
            else if (inSettings)
            {
                MainMenuObject.SetActive(true);
                SettingsMenuObject.SetActive(false);
                inSettings = false;
            }
            
        }
    }

}
