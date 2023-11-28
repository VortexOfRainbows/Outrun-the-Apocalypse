using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject road;

    [SerializeField]
    private int displacementx;

    [SerializeField]
    private int displacementy;

    public Grid grid;
    public Tilemap tilemap;
    public TileBase tile;

    [SerializeField]
    private int x;

    [SerializeField]
    private int y;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f")) 
        {
            tilemap.CompressBounds();
            tilemap.SetTile(new Vector3Int(tilemap.size.x + displacementx, tilemap.size.y + displacementy), tile);

   
        }

        tilemap.CompressBounds();
        x = tilemap.size.x;
        y = tilemap.size.y;
    }

    public int displacement() 
    {
        int x = tilemap.size.x;
        return x;
    }
}
