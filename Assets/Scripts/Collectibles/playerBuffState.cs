using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class playerBuffState : MonoBehaviour
{
    public Player player;
    public ItemData itemdata;
    public Entity entity;

    public float speedMultiplier;
    public float characterSizeMultiplier;
    public float damageMultiplier;
    public float hpincrease;

    public bool collectedBuff;

    public float duration;
    public bool invinciblity;

    private void OnTriggerEnter2D(Collider2D collusion)
    {
        if (collusion.gameObject.tag == "Healing")
        {
            player.Life += hpincrease;
            DamageTextBehavior.SpawnDamageText(Mathf.CeilToInt(hpincrease), transform.position + new Vector3(0, 12));

            if (player.Life > player.MaxLife) { 
            
                player.Life = player.MaxLife;
            }
        }
        else { 
        
            hpincrease = 0;
        }
    }

    void Update()
    {

        if (duration <= 0)
        {
            characterDefaultState();
}
        else if (duration > 0)
        {
            characterBuffedState();
        }

    }
    public void characterDefaultState()
    {
        player.MaxMoveSpeed = 6f;
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        collectedBuff = false;
        characterSizeMultiplier = 1;
        speedMultiplier = 1;
        //hpincrease = 0;

        invinciblity = false;
    }

    public void characterBuffedState() 
    {
        player.MaxMoveSpeed = speedMultiplier;
        gameObject.transform.localScale = new Vector3(1f * characterSizeMultiplier, 1f * characterSizeMultiplier, 1f);
        duration -= Time.deltaTime;

        if (invinciblity == true)
        {
            
        }
    }
}
