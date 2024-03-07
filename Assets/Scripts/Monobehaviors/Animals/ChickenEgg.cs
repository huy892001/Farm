using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenEgg : MonoBehaviour
{
    [SerializeField] ChickenEggItem eggItem;
   
    public ChickenEggItem GetEggItem()
    {
        return eggItem;
    }
}
