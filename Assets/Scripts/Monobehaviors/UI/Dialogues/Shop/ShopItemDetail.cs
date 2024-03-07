using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItemDetail : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtName, txtBuyPrice, txtSellPrice, txtGrowthDuration, txtThirstyPercent, txtDiseasePercent;

    public void Show(CropItem item)
    {
        gameObject.SetActive(true);
        txtName.text = item.name;
        txtBuyPrice.text = item.GetBuyPrice().ToString() + " golds";
        txtSellPrice.text = item.GetSellPrice().ToString() + " golds";
        txtGrowthDuration.text = item.GetGrowthTime().ToString() + "s";
        txtThirstyPercent.text = item.GetThirstyPercent().ToString() + " %";
        txtDiseasePercent.text = item.GetDiseasedPercent().ToString() + " %";
    }
}
