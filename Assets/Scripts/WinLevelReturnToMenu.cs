//Cooper Spring, 1/15/2021, Simple script for the win level button to return player to main menu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLevelReturnToMenu : MonoBehaviour
{
    //send player to main menu
    public void SendToMenu()
    {
        GameManager.Manager.SetLevel(0);
    }
}
