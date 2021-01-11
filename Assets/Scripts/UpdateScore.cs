using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateScore : MonoBehaviour
{
    TextMeshProUGUI tmp;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        Debug.Log(GameManager.Manager);
        GameManager.Manager.OnScoreChanged.AddListener(UpdateText);
    }

    private void UpdateText()
    {
        tmp.text = GameManager.Manager.Score.ToString();
    }
}
