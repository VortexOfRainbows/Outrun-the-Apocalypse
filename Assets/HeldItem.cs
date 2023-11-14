using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class HeldItem : MonoBehaviour
{
    public InventoryItem item;
    public void ItemUpdate()
    {
        if(item == null)
        {
            item = new NoItem();
        }
        item.UpdatePosition();
        transform.localPosition = item.LocalPosition;
        transform.localRotation = item.RotationOffset.ToQuaternion();
        transform.localScale = new Vector3(1, 1, 1) * item.Scale;
        GetComponent<SpriteRenderer>().sprite = SpriteLib.Library.GetSprite("Item", item.SpriteName);
    }
}
