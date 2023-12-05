using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventorySlot : MonoBehaviour
{
    public bool Empty()
    {
        return this.Item is NoItem;
    }
    public bool TryAddingItem(ItemData data)
    {
        if (Empty())
        {
            UpdateItem(data);
            return true;
        }
        return false;
    }
    public ItemData Item;
    [SerializeField] private GameObject parent;
    [SerializeField] private SpriteRenderer Renderer;
    private readonly Vector2 DefaultInventorySlotRoom = new Vector2(22, 22);
    private void Awake()
    {
        Item = new NoItem();
    }
    public void UpdateItem(ItemData data)
    {
        Item = data;

        Vector2 size = data.sprite.rect.size;
        float aspectRatio = size.x / size.y;
        Vector2 stretchFactor = DefaultInventorySlotRoom / size * Utils.PixelsPerUnit; //4 is the pixels per unit
        Vector2 stretchedSize = size * stretchFactor;
        if(stretchedSize.y * aspectRatio > stretchedSize.x) //if the stretch in y is greater than the stretch in x
        {
            //then use the x value for defining the shape
            stretchFactor.y = stretchFactor.x;
        }
        else //if the stretch in x is greater than the strech in y
        {
            //then use the y value for defining the shape
            stretchFactor.x = stretchFactor.y;
        }
        transform.localScale = stretchFactor;

        Vector2 pivot = data.sprite.pivot;
        Renderer.sprite = data.sprite;
        transform.localPosition = (pivot - size * 0.5f) / Utils.PixelsPerUnit * stretchFactor; //4 is out pixels per unit, size * 0.5f is the center of the sprite
    }
}
