using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TunnelGate : MonoBehaviour
{
    public TextMeshProUGUI popupText;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            //Next Level
            GameManager.Manager.NextLevel();
        }
        else if (collision.CompareTag("Player"))
        {
            popupText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            popupText.enabled = false;
        }
    }
}
