using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TunnelGate : MonoBehaviour
{
    public TextMeshProUGUI entrancePopup;
    public TextMeshProUGUI exitPopup;
    public GameObject basementEntrance;
    public GameObject basementExit;

    [SerializeField] bool isInCollider = false;
    [SerializeField] bool inBasement = false;

    private void Start()
    {
        entrancePopup.enabled = false;
        exitPopup.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            entrancePopup.enabled = true;
            exitPopup.enabled = true;
            
        }
        if (Input.GetKeyDown(KeyCode.E) && !inBasement)
        {
            //Next Level
            collision.gameObject.transform.position = basementExit.transform.position;
            inBasement = true;
            print("Entrance");
        }
        else if (Input.GetKeyDown(KeyCode.E) && inBasement)
        {
            //Next Level
            collision.gameObject.transform.position = basementEntrance.transform.position;
            inBasement = false;
            print("Exit");

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            entrancePopup.enabled = false;
            exitPopup.enabled = false;
            isInCollider = false;
        }
    }
}
