//Cooper Spring, 1/14/2021, script to control the color of selected text
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class MouseHoverTMP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    public void OnMouseEnter()
    {
        GetComponent<TextMeshProUGUI>().color = Color.yellow;
    }

    public void OnMouseExit()
    {
        GetComponent<TextMeshProUGUI>().color = Color.white;
    }
}
