using UnityEngine;

public class HeldItem : MonoBehaviour
{
    public ItemData item;
    private ItemData lastItem;
    public void Init()
    {
        if (item == null)
        {
            item = new NoItem();
        }
        GetComponent<SpriteRenderer>().sprite = item.sprite;
    }
    public void ItemUpdate()
    {
        if(item != lastItem)
        {
            Init();
        }
        item.HoldingUpdate();
        transform.localPosition = item.HandOffset;
        transform.localRotation = item.RotationOffset.ToQuaternion();
        transform.localScale = new Vector3(1, 1, 1) * item.Scale;
        lastItem = item;
    }
}
