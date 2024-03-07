using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Cake item")]
public class CakeItem : Item, IExchangeable
{
    [SerializeField] List<ItemHolder> ingredients;
    [SerializeField] float cookDuration;
    // [SerializeField] PriceVariable price;
    // [SerializeField] int sellPrice, buyPrice;

    public List<ItemHolder> GetIngredients()
    {
        return ingredients;
    }

    public float GetCookDuration()
    {
        return cookDuration;
    }
}
