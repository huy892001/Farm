using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlotUI : ItemUI, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Debug.LogError("Inventory item clickedddddddddddddddd");
        SellDialog sellDialog = DialogController.Instance.ShowDialog(DialogType.SELLING, DialogShow.OVER_CURRENT) as SellDialog;
        sellDialog.SetItemHolder(itemHodler);
    }
}
