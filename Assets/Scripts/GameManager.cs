/********************************
 * Author: Flynn Duniho and additions from Thomas Allen
 * Date: 12/18/2020
 * Purpose: Manages scene loading and levels
********************************/

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Manager;

    public AudioMixer Audio;

    public static UnityEvent OnKunaiAmmoChange = new UnityEvent();


    private int _score;
    public int Score
    {
        get => _score;
        set
        {
            if (value != Score)
            {
                _score = value;
                if (OnScoreChanged != null)
                {
                    OnScoreChanged.Invoke();
                }
            }
        }
    }

    public static int KunaiAmmo
    {
        get => ProjectileHandler.kunaiCount;
        set
        {
            ProjectileHandler.kunaiCount = value;
            OnKunaiAmmoChange.Invoke();
        }
    }

    public static int ShurikenAmmo
    {
        get => ProjectileHandler.shurikenCount;
        set
        {
            ProjectileHandler.shurikenCount = value;
            OnKunaiAmmoChange.Invoke();
        }
    }

    public UnityEvent OnScoreChanged = new UnityEvent();

    public static int Level { get; private set; }
    public static int prevLevel { get; private set; }
    public int Volume { get; private set; }
    public bool Fullscreen { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if (Manager != null)
        {
            Destroy(this);
            return;
        }

        Manager = this;

        DontDestroyOnLoad(this);
        var a = GetComponent<AudioSource>();
        if (a != null && !a.isPlaying)
        {
            a.Play();
        }
        Level = SceneManager.GetActiveScene().buildIndex;
        
}

    public void NextLevel()
    {
        SetLevel(Level + 1);
    }

    public void RestartLevel()
    {
        Debug.Log(Level);
        SetLevel(Level);
    }

    public void SetLevel(int level)
    {
        Level = level;
        if (Level > SceneManager.sceneCountInBuildSettings)
        {
            Level = 1;
        }

        Time.timeScale = 1;
        SceneManager.LoadScene(Level);
    }

    public void Exit ()
    {
        Application.Quit();
    }

    public void SetVolume (float v)
    {
        Audio.SetFloat("Volume", v);
    }

    public void SetWindowMode(FullScreenMode f)
    {
        Screen.fullScreenMode = f;
    }

    public void SetWindowMode(int f)
    {
        SetWindowMode((FullScreenMode)f);
    }

    public void SetResolution(int i)
    {
        i = Screen.resolutions.Length - i - 1;
        if (i >= 0 && i < Screen.resolutions.Length)
            Screen.SetResolution(Screen.resolutions[i].width, Screen.resolutions[i].height, Screen.fullScreenMode);
    }

    public void SendToLoseLevel()
    {
        SceneManager.LoadScene("LoseScene");
    }

    public void CreditSceneLoad()
    {
        SceneManager.LoadScene("ScrollingCredits");
    }

    public void MainMenuLoad()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
