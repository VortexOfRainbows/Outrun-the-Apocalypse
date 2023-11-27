using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
   public void PlayGame() 
    {
        SceneManager.LoadScene(2);
    }

    public void QuitGame() 
    { 
        Application.Quit();
    }

    public void GameOver() 
    {
        SceneManager.LoadScene(1);
    }
}
