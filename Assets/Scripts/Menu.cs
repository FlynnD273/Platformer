using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject PauseMenuObject;
    public GameObject SettingsMenuObject;

    protected bool isSettings;


    public virtual void Start()
    {
        PauseMenuObject.SetActive(false);
        SettingsMenuObject.SetActive(false);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                if (isSettings)
                {
                    Settings(false);
                }
                else
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        PauseMenuObject.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1.0f;
        PauseMenuObject.SetActive(false);
    }

    public void Settings (bool s)
    {
        if (s)
        {
            PauseMenuObject.SetActive(false);
            SettingsMenuObject.SetActive(true);
            isSettings = true;
        }
        else
        {
            PauseMenuObject.SetActive(true);
            SettingsMenuObject.SetActive(false);
            isSettings = false;
        }
    }
}