using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Gold item")]
public class GoldItem : Item 
{
    public override int GetQuantityInInventory()
    {
        GoldManager goldManager = FindObjectOfType<GoldManager>();
        if (goldManager != null)
        {
            //Debug.LogError("Current gold: " + goldManager.CurrentGold);
            return goldManager.CurrentGold;
        } else
        {
            Debug.LogError("Gold manager is nulllllllllllllllllll");
        }
        return 0;
    }
}
