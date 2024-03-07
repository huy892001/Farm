using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Missions/Gold")]
public class GoldMission : MissionBase
{
    [SerializeField] ItemHolder requiredItem;
    public override bool CheckIfCompleted()
    {
        GoldManager goldManager = FindObjectOfType<GoldManager>();
        if (goldManager != null)
        {
            if (goldManager.CurrentGold >= requiredItem.Quantity) return true;
            else return false;
        } else
        {
            Debug.LogError("Gold manager is nullllllllllllllll");
        }
        return false;
    }

    public override ItemHolder[] GetItems()
    {
        return new ItemHolder[] { requiredItem }; 
    }
}
