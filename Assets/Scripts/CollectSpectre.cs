using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectSpectre : PreSpawnedItem
{
    public override ItemData SpawnItem => new Spectre();
}
