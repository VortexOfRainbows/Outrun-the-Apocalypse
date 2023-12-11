using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class Wheel : ItemData
{
    public override string StatObjName => "Wheel";
    public override string SpriteName => "Wheel";
    public override Vector2 HandOffset => new Vector2(2.5f, -5.5f);
    public override bool ConsumeAfterUsing(Entity player)
    {
        return true;
    }
    public override void OnUseItem(Entity player)
    {
        DropItem(this);
    }
}
