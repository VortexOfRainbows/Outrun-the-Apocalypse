using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConsumableBehavior : MonoBehaviour
{
    public Consumables consumable;
    public playerBuffState playerBuffState;

    private void OnTriggerEnter2D(Collider2D collusion)
    {

        if (collusion.gameObject.tag == "Player") 
        {
            Destroy(gameObject);

            playerBuffState.collectedBuff = true;
            playerBuffState.duration = consumable.buffduration * Time.timeScale;

            if (consumable.changeSpeed == true)
            {
                playerBuffState.speedMultiplier = consumable.playerSpeedMultiplier;
            }
            if (consumable.changeDamage == true)
            {
                playerBuffState.damageMultiplier = consumable.damageMultiplier;
            }
            if (consumable.changeCharacterSize == true)
            {
                playerBuffState.characterSizeMultiplier = consumable.playerSizeMultiplyer;
                playerBuffState.invinciblity = consumable.invincibility;
            }

            if (consumable.changeHealth == true)
            {
                playerBuffState.hpincrease = consumable.hpincrease;
            }
        }
        

    }

}
