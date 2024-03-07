using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Chicken item")]
public class ChickenItem : Item, IExchangeable, IBuyable 
{
    public int unlockLevel = 10;

    public int GetUnlockLevel() {
        return unlockLevel;
    }
}
