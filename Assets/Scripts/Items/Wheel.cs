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
    }
    public override string SpriteName => "Wheel";
    public override Vector2 HandOffset => new Vector2(2.5f, -5.5f);
    public override bool ChangeHoldAnimation => true;
    public override float RotationOffset => -Mathf.PI / 2;
    public override bool ConsumeAfterUsing => true;
    public override float Scale => 1.0f;
    public override void OnUseItem()
    {
        DropItem(this);
    }
    public override float DeaccelerationRate => 0.95f;
}
