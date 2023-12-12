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
        bool hasPerformedAnUpdate = false;
        for (int i = 0; i < Buttons.Count; i++)
        {
            SlotButton SB = Buttons[i];
            if (SB.PerformUpdate(i))
                hasPerformedAnUpdate = true;
        }
        CursorButton.transform.position = Utils.MouseWorld();
        if (!hasPerformedAnUpdate && Player.MainPlayer.Control.SwapItem && !Player.MainPlayer.LastControl.SwapItem)
        {
            DropCursorItem();
        }
    }
    public void DropCursorItem()
    {
        if(!SlotEmpty(CursorSlot))
        {
            ItemData drop = CursorSlot.Item;
            ItemData.DropItem(drop);
            CursorSlot.UpdateItem(new NoItem());
        }
    }
    public static bool SlotEmpty(InventorySlot slot)
    {
        return slot.Empty();
    }
}
