using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrespawnedPotato : PreSpawnedItem
{
    public override ItemData SpawnItem => new PotatoGun();
}
