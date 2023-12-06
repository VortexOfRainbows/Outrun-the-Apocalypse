using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Stab : ProjectileData
{
    public override string SpriteName => "Stab";
    public override void SetStats()
    {
        Size = new Vector2(20, 20); //Size of the hitbox in pixels
        Lifetime = 12;
        Pierce = 100;
        Friendly = true;
    }
    public override void AfterSpawning(GameObject obj)
    {
        AudioManager.instance.Play("Slash");
        obj.transform.localScale = new Vector3(1f, 0.4f, 1.0f);
    }
}
