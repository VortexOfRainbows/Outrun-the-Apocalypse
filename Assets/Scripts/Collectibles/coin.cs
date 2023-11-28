using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coin : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject prefab;

    // Update is called once per frame

    private void Start()
    {

    }
    void Update()
    {
        //transform.position += new Vector3(-5f * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
            coinCounter.instance.increaseCoins();
        }
    }
}
