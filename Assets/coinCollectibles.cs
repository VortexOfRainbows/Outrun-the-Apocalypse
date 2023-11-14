using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinCollectibles : MonoBehaviour
{

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private int coinCount;

    // Update is called once per frame

    private void Start()
    {
        coinCount = 0;
    }
    void Update()
    {
        //transform.position += new Vector3(-5f * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        coinCount += 1;
        Debug.Log("You have collected " + coinCount + " coins!");
        Destroy(gameObject);
    }

}
