using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectWeapon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
            AudioManager.instance.Play("CoinPickup");
            Player.MainPlayer.AddItemToInventory(new FarmerGun());
        }
    }
}
