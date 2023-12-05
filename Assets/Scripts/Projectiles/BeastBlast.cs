using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastBlast : ProjectileData
{
    public override string SpriteName => "BeastShot";
    public override void SetStats()
    {
        Lifetime = 120;
        Hostile = true;
    }
    public override void OnUpdate(ProjectileObject obj)
    {
        obj.transform.rotation = (obj.Velocity.ToRotation() - 90 * Mathf.Deg2Rad).ToQuaternion();
    }
}