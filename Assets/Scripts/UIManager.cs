using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject InGameUI;
    public GameObject GameOverUI;
    private static UIManager _instance;

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;

            DontDestroyOnLoad(_instance);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        InGameUI.SetActive(false);
        GameOverUI.SetActive(false);
    }
    public void GameOver() 
    { 
        InGameUI.SetActive(false);
        GameOverUI.SetActive(true);
    }

    public void PlayGame()
    {
        InGameUI.SetActive(true);
        GameOverUI.SetActive(false);
    }

}
