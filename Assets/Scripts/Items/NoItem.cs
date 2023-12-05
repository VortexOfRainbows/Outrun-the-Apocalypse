using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class NoItem : ItemData
{
    public override string SpriteName => "None";
    public override Vector2 HandOffset => new Vector2(0, 0);
    public override bool ChangeHoldAnimation => false;
}