/*******************
 * Name: Arya Khubcher
 * Date: 1/12/21
 * Desc: When this script is put into any button, when the button is pushed, it will change the scene from the current scene to the scene its set to go to next.
 *////////////////// 


using UnityEngine;
using UnityEngine.SceneManagement;

public class AryaButtonSceneChanger : MonoBehaviour
{
    public string NextLevel;

    public void Play()
    {
        //load the application
        SceneManager.LoadScene(NextLevel);
    }

    public void Quit()
    {
        //quit the application
        Application.Quit();
    }
}
