using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionUIItem : MonoBehaviour
{
    [SerializeField] Image avatarImg;
    [SerializeField] TextMeshProUGUI quantityTxt;
    [SerializeField] GameObject tickDone;

    ItemHolder cropItem;
    int targetQuantity = 0;

    public void SetQuantity(int currentQuantity, int targetQuantity)
    {
        quantityTxt.text = $"{currentQuantity}/{targetQuantity}";//quantity.ToString();
        this.targetQuantity = targetQuantity;
    }

    public void SetAvatar(Sprite avatarSprite)
    {
        avatarImg.sprite = avatarSprite;
    }

    public void Complete()
    {
        if (tickDone != null) {
            tickDone.SetActive(true);
        }
    }

    public void Uncomplete()
    {
        if (tickDone) {
            tickDone.SetActive(false);
        }
    }

    public void SetCropItem(ItemHolder cropItem)
    {
        this.cropItem = cropItem;
    }

    public void UpdateUI()
    {
        //Debug.Log("Update uiiiiiiiiiiiiiiiiiiiiiiiiiiii: " + cropItem.InventoryItem.Id);
        //ItemHolder cropInInventory = Inventory.Instance.GetItemWithID(cropItem.InventoryItem.Id);
        //if (cropInInventory == null)
        //{
        //    Debug.Log("crop inventory is null");
        //    quantityTxt.text = $"{0}/{targetQuantity}";//quantity.ToString();
        //    Uncomplete();
        //} else
        //{
        //}
        //Debug.Log("crop inventory is not null: " + cropInInventory.Quantity + " " + targetQuantity + " " + cropInInventory.InventoryItem.name);
        int quantityInInventory = cropItem.InventoryItem.GetQuantityInInventory(); 
        quantityTxt.text = $"{quantityInInventory}/{targetQuantity}";//quantity.ToString();
        if (quantityInInventory >= targetQuantity)
        {
            Complete();
        } else
        {
            Uncomplete();
        }
    }

}
