//Abhi, 25/1/2021, Script to control in-game pausing.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPause = false;
    public GameObject PauseMenuUI;
    public GameObject playerUI;
    public GameObject settingsUI;

   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (gameIsPause) { Resume(); }
            else { Pause(); }

    }

    public void Resume() {
        PauseMenuUI.SetActive(false);
        playerUI.SetActive(true);
        Time.timeScale = 1f;
        gameIsPause = false;
        settingsUI.SetActive(false);
       
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        playerUI.SetActive(false);
        Time.timeScale = 0f;
        gameIsPause = true;
    }

    public void Setting() {
        PauseMenuUI.SetActive(false);
        playerUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPause = true;
        settingsUI.SetActive(true);

    }

    public void Back() {
        Pause();
    }

}