using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CakeIngredientUIItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ingreName, curQuantity, targetQuantity;
    [SerializeField] Image avatar;  
    ItemHolder ingredient;

    public void SetIngredientItem(ItemHolder ingredientItem)
    {
        ingredient = ingredientItem;
        avatar.sprite = ingredient.InventoryItem.Avatar;
        //UpdateTextUI();
    }

    public void UpdateTextUI()
    {
        ingreName.text = ingredient.InventoryItem.name;
        curQuantity.text = Inventory.Instance.GetQuantity(ingredient.InventoryItem).ToString(); //ingredient.Quantity.ToString();
        targetQuantity.text = ingredient.Quantity.ToString();
    }
}
