using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variable/Price Variable")]
public class PriceVariable : ScriptableObject 
{
    public int BuyPrice, SellPrice;
}
