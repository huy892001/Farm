using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] CropItem crop;

    public void AddCrop()
    {
        Inventory.Instance.AddItem(crop);
    }
}
