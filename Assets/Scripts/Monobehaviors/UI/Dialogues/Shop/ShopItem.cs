using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sellPriceText, buyPriceText, itemName;
    [SerializeField] Image avatar;
    [SerializeField] GameObject lockMask;
    [SerializeField] TextMeshProUGUI levelOpenTxt;
    [SerializeField] RectTransform floatingTextPrefab;

    //[SerializeField] GameEvent onItemBuyedSuccess;

    Item item;

    private void Start()
    {
        // IExchangeable price = item as IExchangeable; 
        sellPriceText.text = item.GetSellPrice().ToString();
        buyPriceText.text = item.GetBuyPrice().ToString();
        itemName.text = item.Name;
        avatar.sprite = item.Avatar;
        IBuyable unlockLevel = item as IBuyable; 
        if (unlockLevel.GetUnlockLevel() <= LevelManager.Instance.GetCurrentLevel())
        {
            lockMask.SetActive(false);
        } else
        {
            lockMask.SetActive(true);
            levelOpenTxt.text = "Level unlock: " + unlockLevel.GetUnlockLevel().ToString();
        }
    }

    public void SetItem(Item itemToSet)
    {
        item = itemToSet;
    }

    public void BuyItem()
    {
        GoldManager goldManager = FindObjectOfType<GoldManager>();
        SeedItem seedItem = item as SeedItem;
        ChickenItem chickenItem = item as ChickenItem;
        // IExchangeable price = item as IExchangeable;
        if (goldManager.HasEnoughGold(item.GetBuyPrice()))
        {
            goldManager.SubtractGold(item.GetBuyPrice());
            if (seedItem)
            {
                Inventory.Instance.AddItem(seedItem);
            } else if (chickenItem) {
                Debug.Log("Buy chickennnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn");
                // FindObjectOfType<ChickenController>().SpawnChicken();
                ChickenController chickenController = FindObjectOfType<ChickenController>();
                if (chickenController != null) {
                    Debug.Log("Spawn chicken");
                    chickenController.SpawnChicken();
                }

            } 
            FloatingUIItemController.Instance.Show(item.Avatar, 1, avatar.transform.position - Vector3.up * 20, avatar.transform.position + Vector3.up * 30);
        } else
        {
            ToastManager.Instance.ShowNotifyWorldPosition("NOT ENOUGH MONEY", transform.position);
        }
    }

    public void ShowDetail()
    {
        SeedItem seedItem = item as SeedItem;
        if (seedItem == null)
        {
            Debug.LogError("Seed item is nullllllllllllllllll");
        } else
        {
            if (seedItem.GetCropItem() == null) {
                Debug.LogError("crop item in seed is null");
            } else {
                Debug.LogError("Vo li vkllllllllllllllll");
            }
            FindObjectOfType<ShopDialogue>().ShowDetail(seedItem.GetCropItem());
        }
    }
}
