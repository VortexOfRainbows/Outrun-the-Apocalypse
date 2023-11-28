using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float spawnMinTime; 
    [SerializeField]
    private float spawnMaxTime;

    private float nextSpawnTime;
    private float lastSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnTime = Time.time;
        nextSpawnTime = Random.Range(spawnMinTime, spawnMaxTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastSpawnTime + nextSpawnTime)
        {
            Instantiate(prefab, new Vector3(0f, 0f, 0), transform.rotation);
            nextSpawnTime = Time.time + Random.Range(spawnMinTime, spawnMaxTime); // Redefines and reandomizes the next spawn time
        }
    }
}
