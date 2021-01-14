//Cooper Spring, 1/5/2021, Script to control the settings back button
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsBackButton : MonoBehaviour
{
    public GameObject MainMenuObject;
    public GameObject SettingsMenuObject;
    private void OnMouseUp()
    {
        MainMenuObject.SetActive(true);
        SettingsMenuObject.SetActive(false);
    }

    public void OnMouseEnter()
    {
        GetComponent<TMP_Text>().color = Color.yellow;
    }

    public void OnMouseExit()
    {
        GetComponent<TMP_Text>().color = Color.white;
    }
}
