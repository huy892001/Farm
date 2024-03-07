using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CakeUIItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cakeName, sellPrice, cookTime;
    [SerializeField] CakeIngredientUIItem cakeIngredientPrefab;
    [SerializeField] RectTransform cakeIngreParent;
    [SerializeField] CakeItem cakeItem;
    [SerializeField] Image avatarImg;
    // [SerializeField] Button cookButton;

    public List<CakeIngredientUIItem> cakeIngredients = new List<CakeIngredientUIItem>();
    bool hasInited = false;

    private void Start()
    {
        cakeName.text = cakeItem.name;
        sellPrice.text = cakeItem.GetSellPrice().ToString();
        cookTime.text = cakeItem.GetCookDuration().ToString();
        avatarImg.sprite = cakeItem.Avatar;
    }

    public void UpdateUI()
    {
        if (!hasInited)
        {
            hasInited = true;
            InitIngredientUIItems();
        } else
        {
            for (int i = 0; i < cakeIngredients.Count; i++)
            {
                cakeIngredients[i].UpdateTextUI();
            }
        } 
    }
    public void InitIngredientUIItems()
    {
        Debug.LogError("init ingredient: " + cakeItem.GetIngredients().Count);
        List<ItemHolder> ingres = cakeItem.GetIngredients();
        for (int i = 0; i < ingres.Count; i++)
        {
            CakeIngredientUIItem newIngre = Instantiate(cakeIngredientPrefab, cakeIngreParent);
            newIngre.SetIngredientItem(ingres[i]);
            newIngre.UpdateTextUI();
            cakeIngredients.Add(newIngre);
        }
    }
    public bool HasEnoughIngredients()
    {
        List<ItemHolder> ingres = cakeItem.GetIngredients();
        for (int i = 0; i < ingres.Count; i++)
        {
            if (Inventory.Instance.GetQuantity(ingres[i].InventoryItem) < ingres[i].Quantity) return false;
        }
        return true;
    }
    public void Cook()
    {
        if (HasEnoughIngredients())
        {
            Debug.LogError("Enough ingredient");
            List<ItemHolder> ingres = cakeItem.GetIngredients();
            for (int i = 0; i < ingres.Count; i++)
            {
                Inventory.Instance.SubtractQuantity(ingres[i].InventoryItem, ingres[i].Quantity);
                UpdateUI();
            }
            if (cakeItem == null) {
                Debug.LogError("Cake item is null in cake ui itemmmmmmmmmmmmmm");
            }
            FindObjectOfType<CookingDialog>().ShowCookingPanel(cakeItem, cakeItem.GetCookDuration());
        } else
        {
            // ToastManager.Instance.ShowNotifyRect("NOT ENOUGH CROP", GetComponent<RectTransform>().anchoredPosition);
            ToastManager.Instance.ShowNotifyWorldPosition("NOT ENOUGH CROP", transform.position);
            Debug.LogError("Not Enough ingredientttttttttttttt");
        }
    }
}
