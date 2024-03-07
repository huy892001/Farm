using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopDialogue : Dialog 
{
    [SerializeField] List<Item> items;
    [SerializeField] ShopItem shopItemSlotPrefab;
    [SerializeField] RectTransform itemParent;
    [SerializeField] ScrollRect scroll;
    [SerializeField] ShopItemDetail shopItemDetail;

    List<ShopItem> shopUIItems = new List<ShopItem>();

    protected override void Start()
    {
        base.Start();
        scroll.enabled = false;
        FindObjectOfType<GoldDisplayer>().Show();
        Debug.Log("Shop item count: " + items.Count);
        for (int i = 0; i < items.Count; i++)
        {
            ShopItem newItem = Instantiate(shopItemSlotPrefab, itemParent);
            newItem.SetItem(items[i]);
            shopUIItems.Add(newItem);
        }
        onDialogCompleteShowing = () =>
        {
            scroll.enabled = true;
            Debug.LogError("On dialog complete showing");
        };
    }
    public void ShowDetail(CropItem crop)
    {
        shopItemDetail.Show(crop);
    }

    public override void Close()
    {
        base.Close();
        FindObjectOfType<GoldDisplayer>().Hide();
    }
}
