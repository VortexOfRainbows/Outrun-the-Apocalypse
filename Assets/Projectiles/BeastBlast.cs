using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastBlast : ProjectileData
{
    public override string SpriteName => "BeastShot";
    public override void SetStats()
    {
        Lifetime = 120;
    }
}
