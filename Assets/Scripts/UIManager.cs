using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject InGameUI;
    public GameObject GameOverUI;
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
