using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem
{
    public Vector2 LocalPosition;
    public void UpdatePosition()
    {
        LocalPosition = HandOffset;
    }
    public virtual string SpriteName { get; }
    public virtual Vector2 HandOffset { get; }
    public virtual bool ChangeHoldAnimation => false;
    public virtual float RotationOffset => 0f;
    public virtual float Scale => 0.75f;
}