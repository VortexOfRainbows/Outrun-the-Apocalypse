using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public const float Second = 60f;   
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private float spawnMinTime; 
    [SerializeField]
    private float spawnMaxTime;
    private float nextSpawnTime;
    void Start()
    {
        nextSpawnTime = Random.Range(spawnMinTime, spawnMaxTime) * Second;
    }
    private const float spawnRange = 240; //16 units in a tile = 15 blocks radius
    private void FixedUpdate()
    {
        if ((Player.MainPlayer.Position - (Vector2)transform.position).magnitude < spawnRange * Entity.EnemyScalingFactor)
        {
            if (nextSpawnTime <= 0) //only spawn if player is within units
            {
                Instantiate(prefab, new Vector2(transform.position.x, transform.position.y), new Quaternion());
                nextSpawnTime = Random.Range(spawnMinTime, spawnMaxTime) * Second; // Redefines and reandomizes the next spawn time
            }
            nextSpawnTime -= 0.5f + 0.5f * Entity.EnemyScalingFactor; //Only effected by half of the spawn speed
        }
        else
            nextSpawnTime -= 0.1f; //Spawn speed is reduced to 1/10 if outside range
    }
}
