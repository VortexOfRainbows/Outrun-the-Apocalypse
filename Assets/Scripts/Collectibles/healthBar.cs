using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    // Start is called before the first frame update

    public Slider healthSlider;
    public int health = 100;

    public void SetMaxHealth()
    {
        healthSlider.maxValue = health; 
        healthSlider.value = health;
    
    }

    public void setHealth() 
    {

        healthSlider.value = health;

    }

    private void Update()
    {
        healthSlider.value = health;

        if (Input.GetKeyDown(KeyCode.Space)) 
        { 
            health -= 10;
            Debug.Log(health);
        }
    }

}
