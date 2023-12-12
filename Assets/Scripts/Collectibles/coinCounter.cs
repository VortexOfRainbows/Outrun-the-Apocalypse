using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter instance;
    public TMP_Text coinText;
    public int CoinCount { get; private set; }
    void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if(instance != this || instance == null)
            instance = this;
    }
    private void Start()
    {
        coinText.text = "Coins:  " + CoinCount.ToString();
    }
    public void ChangeCoins(int changeAmount = 1)
    {
        CoinCount += changeAmount;
        coinText.text = "Coins:  " + CoinCount.ToString();
    }
}
