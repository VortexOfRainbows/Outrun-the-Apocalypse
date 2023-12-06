using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUIScript : MonoBehaviour
{
    public UIManager manager;
    public GameObject PauseUI;

    public static bool GameIsPaused = false;

    private void Start()
    {
        PauseUI.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameIsPaused)
            {
                Pause();
            }
            else 
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        GameIsPaused = true;
        Time.timeScale = 0f;
        PauseUI.SetActive(true);
    }

    public void Resume()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        PauseUI.SetActive(false);
    }
}
