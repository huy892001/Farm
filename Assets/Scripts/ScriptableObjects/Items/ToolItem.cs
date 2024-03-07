using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolItem : Item, IEquipmentable
{
    [SerializeField] GameObject prefab;
    public GameObject SpawnItem()
    {
        return Instantiate(prefab);
    }

}
