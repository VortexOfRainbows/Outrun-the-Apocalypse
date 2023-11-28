using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterState : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject character;
    public GameStateManager GameStateManager;
    public UIManager UIManager;
    public int health;
    public int maxhealth;

    void Start()
    {
        UIManager.PlayGame();
        health = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            //Destroy(gameObject);
            UIManager.GameOver();
        }
        else {
            UIManager.PlayGame();
        }
    }
}
