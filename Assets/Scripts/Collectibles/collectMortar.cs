using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectMortar : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
            AudioManager.instance.Play("CoinPickup");
            Player.MainPlayer.AddItemToInventory(new MortarCannon());
        }
    }
}
