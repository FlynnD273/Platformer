//Cooper Spring, 11/5/2020, Script to control the pause menu's buttons
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuButtons : MonoBehaviour
{
    //button bools
    public bool resumeSelect;
    public bool quitSelect;
    public static bool buttonPressed;
    private int timesClicked = 0;
    public GameObject checkText;
    private Text checkTextComp;
    private void Start()
    {
        checkTextComp = checkText.GetComponent<Text>();
        checkTextComp.text = "";
    }

    private void OnMouseUp()
    {
        //checks if a button(the text specifically, put this on the text)
        //was clicked with the start or quit select variable checked, 
        //which is passed because they have a trigger collider
        if (resumeSelect)
        {
            //tell the pause menu that we need to unpause
            PauseMenu.isPaused = false;
            timesClicked = 0;
        }

        if (quitSelect)
        {
            timesClicked++;

            if(timesClicked != 2)
            {
                checkTextComp.text = "Are You Sure?";
            }
            else
            {
                GameManager.Manager.SetLevel(0);
                timesClicked = 0;
            }
        }

        buttonPressed = true;
    }
}
