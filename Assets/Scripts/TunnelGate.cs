using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TunnelGate : MonoBehaviour
{
    public TextMeshProUGUI entrancePopup;
    public TextMeshProUGUI exitPopup;
    public GameObject basementEntrance;
    public GameObject basementExit;

    [SerializeField] bool inBasement = false;

    public Vector3 playerPos;
    public Vector3 doorPos;

    public GameObject player;

    private void Start()
    {
        entrancePopup.enabled = false;
        exitPopup.enabled = false;        
    }

    private void Update()
    {
        if (player.transform.position.x < 0)
            inBasement = true;
        else
            inBasement = false;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            entrancePopup.enabled = true;
            exitPopup.enabled = true; 
        }
        if (Input.GetKey(KeyCode.F))
        {
            if (inBasement)
            {
                player.transform.position = basementEntrance.transform.position;
            }
            else
            {
                player.transform.position = basementExit.transform.position;
            }
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            entrancePopup.enabled = false;
            exitPopup.enabled = false;
        }
    }
}
