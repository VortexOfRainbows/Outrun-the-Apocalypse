using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    // Start is called before the first frame update

    public Slider healthSlider;
    public characterState characterState;

    public void SetMaxHealth()
    {
        healthSlider.maxValue = characterState.maxhealth; 
        healthSlider.value = characterState.health;
    
    }

    public void setHealth() 
    {

        healthSlider.value = characterState.health;

    }

    private void Update()
    {
        healthSlider.value = characterState.health;

        if (Input.GetKeyDown(KeyCode.Space)) 
        { 
            characterState.health -= 10;
            //Debug.Log(health);
        }
    }

}
