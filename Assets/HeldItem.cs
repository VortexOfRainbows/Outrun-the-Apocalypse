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
    public void ItemUpdate(Entity holdingEntity)
    {
        if(item != lastItem)
        {
            Init();
        }
        item.HoldingUpdate(holdingEntity);
        transform.localPosition = item.HandOffset;
        transform.localRotation = item.GetHoldOutRotation.ToQuaternion();
        transform.localScale = new Vector3(1, 1, 1) * item.GetScale;
        lastItem = item;
    }
}
