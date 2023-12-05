using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] public List<SlotButton> Buttons;
    [SerializeField] public List<InventorySlot> Slot;
    [SerializeField] public GameObject CursorButton;
    [SerializeField] public InventorySlot CursorSlot;
    public ItemData CursorItem => CursorSlot.Item;
    /// <summary>
    /// Updates the collision for inventory slot buttons
    /// </summary>
    public void PerformUpdate()
    {
        foreach(SlotButton SB in Buttons)
        {
            SB.PerformUpdate();
        }
        CursorButton.transform.position = Utils.MouseWorld();
        if((Player.MainPlayer.Control.LeftClick && !Player.MainPlayer.LastControl.LeftClick)
            || (Player.MainPlayer.Control.RightClick && !Player.MainPlayer.LastControl.RightClick))
        {
            DropCursorItem();
        }
    }
    public void DropCursorItem()
    {
        if(!SlotEmpty(CursorSlot))
        {
            Vector2 toMouse = Utils.MouseWorld() - Player.MainPlayer.Position;
            ItemData.NewItem(CursorSlot.Item, Player.MainPlayer.Position, toMouse.normalized * 8.5f);
            CursorSlot.UpdateItem(new NoItem());
        }
    }
    public static bool SlotEmpty(InventorySlot slot)
    {
        return slot.Empty();
    }
}
