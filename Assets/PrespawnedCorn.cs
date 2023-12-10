using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PrespawnedCorn : PreSpawnedItem
{
    public override ItemData SpawnItem => new Corn();
}
