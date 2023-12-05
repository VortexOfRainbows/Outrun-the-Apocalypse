using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class Wheel : ItemData
{
    public override void SetStats()
    {
        Damage = 0;
        Size = new Vector2(11, 9);
        Scale = 1.0f;
        DeaccelerationRate = 0.95f;
        ChangeHoldAnimation = true; RotationOffset = -Mathf.PI / 2;
    }
    public override string SpriteName => "Wheel";
    public override Vector2 HandOffset => new Vector2(2.5f, -5.5f);
    public override bool ConsumeAfterUsing(Player player)
    {
        return true;
    }
    public override void OnUseItem()
    {
        DropItem(this);
    }
}
