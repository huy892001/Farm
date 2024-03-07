using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDialog : Dialog 
{
    [SerializeField] Inventory inventory;
    [SerializeField] ItemSlotUI itemSlotPrefab;
    [SerializeField] Transform productParent;
    [SerializeField] Transform seedParent;
    [SerializeField] Image farmProductsImage;
    [SerializeField] Image seedsImage;
    [SerializeField] Sprite normalSprite, activeSprite;

    List<ItemSlotUI> itemSlots = new List<ItemSlotUI>();
    bool seedView = true;

    private void OnEnable()
    {
        OnFarmProductViewClicked();
    }

    public void InitItemSlots()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            Destroy(itemSlots[i].gameObject);
        }
        itemSlots.Clear();
        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i].Quantity <= 0 || !inventory.items[i].InventoryItem.VisibleInInventory) continue;
            ItemSlotUI itemSlotInstance = Instantiate(itemSlotPrefab);
            SeedItem seedItem = inventory.items[i].InventoryItem as SeedItem;
            if (seedItem)
            {
                itemSlotInstance.transform.SetParent(seedParent);
            } else
            {
                itemSlotInstance.transform.SetParent(productParent);
            }
            itemSlotInstance.transform.localScale = Vector3.one;
            itemSlotInstance.SetItemHolder(inventory.items[i]);
            itemSlots.Add(itemSlotInstance);
        }
    }

    // btn seed view event
    public void OnSeedViewClicked()
    {
        Debug.LogError("Seed view clickeddddddddddddd");
        seedsImage.sprite = activeSprite;
        farmProductsImage.sprite = normalSprite;
        productParent.gameObject.SetActive(false);
        seedParent.gameObject.SetActive(true);
        InitItemSlots();
    }
    // btn seed view event
    public void OnFarmProductViewClicked()
    {
        Debug.LogError("Farm product view clickeddddddddddddd");
        seedsImage.sprite = normalSprite;
        farmProductsImage.sprite = activeSprite;
        productParent.gameObject.SetActive(true);
        seedParent.gameObject.SetActive(false);
        InitItemSlots();
    }

    public override void Close()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO)
        {
            playerGO.GetComponent<PlayerMovement>().SetMovingState(true);
        }
        base.Close();
    }
}
