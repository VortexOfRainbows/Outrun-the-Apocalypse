using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPotato : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
            AudioManager.instance.Play("CoinPickup");
            Player.MainPlayer.AddItemToInventory(new PotatoGun());
        }
    }
}
