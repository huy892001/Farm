using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Inventory : ScriptableObject
{
    public List<ItemHolder> items;
    public static Inventory _instance;
    const string resourcePath = "Inventory";

    public GameEvent onItemsChanged;

    public static Inventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<Inventory>(resourcePath);
            }
            if (_instance == null)
            {
                Debug.LogError("Can not find inventor resourse in : " + resourcePath);
            }
            return _instance;
        }
    }

    public void Reset()
    {
        items.Clear();
    }

    public void AddItem(Item itemToAdd, int quantity = 1)
    {
        if (itemToAdd == null)
        {
            Debug.LogError("Item to add is null");
        }
        int itemToAddIndex = GetItemIndex(itemToAdd);
        if (itemToAddIndex != -1)
        {
            items[itemToAddIndex].Quantity += quantity;
        } else
        {
            items.Add(new ItemHolder() { InventoryItem = itemToAdd, Quantity = quantity });
        }
        // Debug.LogError("Add itemmmmmmmmmmmmmmmmmm: " + itemToAdd.name);
        if (onItemsChanged) onItemsChanged.NotifyAll();
    }
    public IEnumerable<ItemHolder> GetAllItems()
    {
        return items; 
    }

    public ItemHolder GetItemWithID(string id)
    {
        //Debug.Log("Inventory length: " + items.Count);
        //Debug.LogError("Get item id: " + id);
        foreach (var item in items)
        {
            if (item == null)
            {
                Debug.Log("[Inventory] Item is null");
            }
            if (item.InventoryItem == null)
            {
                Debug.Log("Inventory item is null");
            }
            if (item.InventoryItem.Id == id)
            {
                return item;
            }
        }
        return null;
    }

    public bool HasItem(Item item)
    {
        return GetItemWithID(item.Id) != null;
    } 

    public void SubtractQuantity(Item item, int quantity = 1)
    {
        int itemToAddIndex = GetItemIndex(item);
        if (itemToAddIndex != -1)
        {
            if (items[itemToAddIndex].Quantity >= quantity)
            {
                items[itemToAddIndex].Quantity -= quantity;
                if (onItemsChanged) onItemsChanged.NotifyAll();
            }
        }
    }
    public int GetQuantity(Item item)
    {
        int itemIndex = GetItemIndex(item); 
        if (itemIndex != -1)
        {
            return items[itemIndex].Quantity;
        }
        return 0;
    }
    public int Count()
    {
        int cnt = 0;
        foreach (var item in items)
        {
            if (item.Quantity > 0) cnt++;
        }
        return cnt;
    } 
    int GetItemIndex(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].InventoryItem == item) return i;
        }
        return -1;
    }
}
