using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public void Awake()
    {
        gameObject.SetActive(false);
    }
    public void OnDestroy()
    {
        
    }
}
