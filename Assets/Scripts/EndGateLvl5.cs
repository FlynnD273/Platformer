using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGateLvl5 : MonoBehaviour
{
    public GameObject player;

    public TextMeshProUGUI withKeyPopup;
    public TextMeshProUGUI withoutKeyPopup;

    AsyncOperation async;
    // Start is called before the first frame update
    private void Start()
    {
        withKeyPopup.enabled = false;
        withoutKeyPopup.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (NEWPlayerLogic.hasKey)
                withKeyPopup.enabled = true;

            else
                withoutKeyPopup.enabled = true;
        }
        
        if (Input.GetKey(KeyCode.Return) && NEWPlayerLogic.hasKey)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            withKeyPopup.enabled = false;
            withoutKeyPopup.enabled = false;
        }
    }

}
