using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemQuantityText;
    protected ItemHolder itemHodler;
    
    public void SetItemHolder(ItemHolder itemHolderParam)
    {
        itemHodler = itemHolderParam;
        itemImage.sprite = itemHodler.InventoryItem.Avatar;
        itemQuantityText.text = itemHodler.Quantity.ToString();
    }

    public Item GetItem()
    {
        return itemHodler.InventoryItem;
    }
}
