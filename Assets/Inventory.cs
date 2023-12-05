using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] public List<InventorySlot> Slot;
    public static bool SlotEmpty(InventorySlot slot)
    {
        return slot.Empty();
    }
}
