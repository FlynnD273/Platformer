using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScreenReplay : MonoBehaviour
{
    public void Restart()
    {
        GameManager.Manager.RestartLevel();
    }
}
