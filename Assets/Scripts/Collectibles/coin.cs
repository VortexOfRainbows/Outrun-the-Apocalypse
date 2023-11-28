using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    void Update()
    {
        //transform.position += new Vector3(-5f * Time.deltaTime, 0f, 0f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
            CoinCounter.instance.increaseCoins();
            AudioManager.instance.Play("CoinPickup");
        }
    }
}
