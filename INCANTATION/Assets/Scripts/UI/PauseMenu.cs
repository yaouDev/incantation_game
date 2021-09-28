using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;

    public GameObject pauseMenuUI;
    private float storedTimeScale;

    private GameManager gm;

    private void Start()
    {
        gm = GameManager.instance;
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = storedTimeScale;
        isPaused = false;
        gm.isInputEnabled = true;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        storedTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        isPaused = true;
        gm.isInputEnabled = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        //VVV uncomment later
        //SceneManager.LoadScene("Menu");
    }
}
