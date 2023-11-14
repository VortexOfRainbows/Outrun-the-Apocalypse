using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class coinCounter : MonoBehaviour
{
    
    public static coinCounter instance;
    public TMP_Text coinText; 
    public int coinIndex;
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        coinText.text = "Coins:  " + coinIndex.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void increaseCoins()
    {
        coinIndex += 1;
        coinText.text = "Coins:  " + coinIndex.ToString();

    }
}
