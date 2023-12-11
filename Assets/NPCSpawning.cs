using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCSpawning : MonoBehaviour
{
    private Tilemap Tilemap;
    public void EstablishTilemapReference()
    {
        if(Tilemap == null)
        {
            TilemapCollider2D maps = FindObjectOfType<TilemapCollider2D>(); //this is the easiest way to consistenly find the tilemap with collision, as scenes may organize maps differently, and the tilemap is hard to serialize (as NPCSpawning belongs to the player, which is tied to something else on scene loading)
            if(maps != null)
                Tilemap = maps.gameObject.GetComponent<Tilemap>();
        }
    }
    public bool HasCollide(Vector2 position)
    {
        Vector3Int VectorAsInt = new Vector3Int((int)position.x, (int)position.y);
        return Tilemap.HasTile(VectorAsInt);
    }
    private Vector2 PlayerLocation => Player.MainPlayer.Position;
    [SerializeField] private float SpawnTimerCounter = 0;
    [SerializeField] private float MaximumSpawnTime = 200;
    [SerializeField] private float MinimumSpawnTime = 20;
    [SerializeField] private int LocationAttemptsPerCycle = 50;
    [SerializeField] private float SecondsUntilMaxSpawns = 600;
    /// 
    /// The screen displayed to the player is about 210 in each x direction.
    /// It is about 120 in each y direction
    ///
    [SerializeField] private int MinimumDistanceY = 160;
    [SerializeField] private int MaximumDistanceY = 260;
    [SerializeField] private int MinimumDistanceX = 250;
    [SerializeField] private int MaximumDistanceX = 350;
    [SerializeField] private float SpawnTimeVariance = 0.1f;
    private float nextSpawnTime = 60;
    public float GetNextSpawnTime()
    {
        int currentTime = Timer.RawSeconds;
        float speedUpSpawnTimes = currentTime / SecondsUntilMaxSpawns;
        return Mathf.Lerp(MaximumSpawnTime, MinimumSpawnTime, speedUpSpawnTimes) * Random.Range(1 - SpawnTimeVariance, 1 + SpawnTimeVariance);
    }
    public void FixedUpdate()
    {
        EstablishTilemapReference();
        SpawnTimerCounter++;
        if(SpawnTimerCounter >= nextSpawnTime)
        {
            SpawnEnemy();
            SpawnTimerCounter -= nextSpawnTime;
            nextSpawnTime = GetNextSpawnTime();
        }
    }
    public void SpawnEnemy()
    {
        Vector2? spawnLocation = FindLocationForSpawning();
        if (spawnLocation != null)
        {
            Instantiate(DetermineEnemy(), (Vector3)spawnLocation, Quaternion.identity);
        }
    }
    public Vector2? FindLocationForSpawning()
    {
        for(int i = LocationAttemptsPerCycle; i > 0; i--)
        {
            int yDirection = Random.Range(0, 2) * 2 - 1; //Picks a number either -1 or 1
            int xDirection = Random.Range(0, 2) * 2 - 1;
            int rangeX = Random.Range(0, MaximumDistanceX);
            int rangeY = Random.Range(0, MaximumDistanceY);
            if (rangeX >= MinimumDistanceX || rangeY >= MinimumDistanceY)
            {
                Vector2 SpawnLocationOffset = new Vector2(rangeX * xDirection, rangeY * yDirection);
                Vector2 SpawnLocation = SpawnLocationOffset + PlayerLocation;
                if (Tilemap == null || !HasCollide(SpawnLocation))
                {
                    return SpawnLocation;
                }
            }
        }
        return null;
    }
    public GameObject DetermineEnemy()
    {
        //In here, we might want to give zombies the ability to wield weapons
        return PrefabManager.GetPrefab("zombie");
    }
}
