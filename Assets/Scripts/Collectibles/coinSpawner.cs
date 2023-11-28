using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinSpawner : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private GameObject prefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Instantiate(prefab, transform.position + new Vector3(Random.Range(0, 150.0f), Random.Range(-100.0f, 100.0f), 0), transform.rotation);
        //coinCounter.instance.increaseCoins();
    }

}
