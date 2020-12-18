/********************************
 * Author: Flynn Duniho
 * Date: 12/18/2020
 * Purpose: Manages scene loading and levels
********************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Manager;

    public static int Level { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (Manager != null)
        {
            Destroy(Manager.gameObject);
        }

        Manager = this;
    }

    public void NextLevel()
    {
        SetLevel(Level + 1);
    }

    public void RestartLevel()
    {
        SetLevel(Level);
    }

    public void SetLevel(int level)
    {
        Level = level;
        if (Level > SceneManager.sceneCountInBuildSettings)
        {
            Level = 1;
        }

        SceneManager.LoadScene(Level);
        
    }
}
