using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class NoItem : InventoryItem
{
    public override string SpriteName => "None";
    public override Vector2 HandOffset => new Vector2(0, 0);
    public override bool ChangeHoldAnimation => false;
}
public class FarmerGun : InventoryItem
{
    public override string SpriteName => "FarmerGun";
    public override Vector2 HandOffset => new Vector2(1.5f, -4.5f);
    public override bool ChangeHoldAnimation => true;
    public override float RotationOffset => -Mathf.PI / 2;
}
