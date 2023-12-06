using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreSpawnedKey : PreSpawnedItem
{
    public override ItemData SpawnItem => new Key();
}
