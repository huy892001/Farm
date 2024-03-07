using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Item")]
public class ItemMission : MissionBase
{
    public ItemHolder[] requiredItems;
    public override bool CheckIfCompleted()
    {
        foreach (var requiredItem in requiredItems)
        {
            if (!IsItemCompleted(requiredItem)) return false;
        }
        return true;
    }
    public bool IsItemCompleted(ItemHolder crop)
    {
        // Debug.LogError("Inventory count: " + Inventory.Instance.GetAllItems());
        foreach (var item in Inventory.Instance.GetAllItems())
        {
            if (item.InventoryItem.Id == crop.InventoryItem.Id)
            {
                if (item.Quantity >= crop.Quantity) return true;

            }
        }
        return false;
    }

    public override ItemHolder[] GetItems()
    {
        return requiredItems;
    }

    //public override void UpdateUI()
    //{
    //}
}
