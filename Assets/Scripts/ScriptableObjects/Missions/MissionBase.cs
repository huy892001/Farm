using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MissionBase : ScriptableObject
{
    public abstract bool CheckIfCompleted();
    public abstract ItemHolder[] GetItems();

    //public abstract void UpdateUI();
    //public abstract GameObject GetUIGameObject();
}
