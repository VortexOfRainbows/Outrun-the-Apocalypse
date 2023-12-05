using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreSpawnedFarmerGun : PreSpawnedItem
{
    public override ItemData SpawnItem => new FarmerGun();
    public override Vector2 SpawnVelocity()
    {
        Vector2 random = new Vector2(1, 0).RotatedBy(Random.Range(0, Mathf.PI * 2));
        return random;
    }
}
