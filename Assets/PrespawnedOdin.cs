using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrespawnedOdin : PreSpawnedItem
{
    public override ItemData SpawnItem => new Odin();
}
