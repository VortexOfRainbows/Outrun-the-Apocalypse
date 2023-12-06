using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreSpawnedFarmerGun : PreSpawnedItem
{
    public override ItemData SpawnItem => new FarmerGun();
}
