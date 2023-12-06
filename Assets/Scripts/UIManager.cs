using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static bool GameEnd => instance._GameOver;
    public static bool GameIsPaused = false;

    // Start is called before the first frame update
    public GameObject InGameUI;
    public GameObject GameOverUI;
    public GameObject PauseUI;

    private bool _GameOver = false;
    public bool win = false;

    public static UIManager instance;
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        InGameUI.SetActive(false);
        GameOverUI.SetActive(false);
        PauseUI.SetActive(false);
        GameIsPaused = false;
        win = false;
    }
    public void Update()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _GameOver == false)
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

        Debug.Log(_GameOver);
    }
    public void GameWon()
    {
        _GameOver = true;
        win = true;
        GameOverUI.GetComponentInChildren<TextMeshProUGUI>().text = "ESCAPE SUCCESSFUL";
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
    }
    public void GameOver() 
    {
        _GameOver = true;
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
    }
    public void PlayGame()
    {
        _GameOver = false;
        InGameUI.SetActive(true);
        GameOverUI.SetActive(false);
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
