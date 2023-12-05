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
    }
    public static bool SlotEmpty(InventorySlot slot)
    {
        return slot.Empty();
    }
}
