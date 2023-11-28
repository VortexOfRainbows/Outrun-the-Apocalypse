using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainSinglton : MonoBehaviour
{
    // Start is called before the first frame update

    private static MainSinglton _instance;

    void Start()
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
    }
}
