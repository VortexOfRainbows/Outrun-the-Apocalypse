using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    [SerializeField]
    private GameObject Button;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Button.SetActive(true);  
      
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Button.SetActive(false);
    }
}

