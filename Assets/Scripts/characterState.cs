using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterState : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject character;
    public GameStateManager GameStateManager;
    public int health;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) 
        {
            Destroy(gameObject);
            GameStateManager.GameOver();
        }
    }
}
