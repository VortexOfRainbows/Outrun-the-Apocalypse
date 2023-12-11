using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrespawnedMortar : PreSpawnedItem
{
    public override ItemData SpawnItem => new MortarCannon();
}
