using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static bool GameEnd => instance._GameOver;
    // Start is called before the first frame update
    public GameObject InGameUI;
    public GameObject GameOverUI;

    private bool _GameOver = false;

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
    }
    public void Update()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        }
    }
    public void GameWon()
    {
        _GameOver = true;
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
}
