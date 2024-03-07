using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crop Thirsty State", menuName = "Crop States/Thirsty")]
public class CropThirstyState : BaseState
{
    public override void Enter(Crop crop)
    {
        crop.DisplayThirstyIcon();
    }

    public override void Execute(Crop crop)
    {
    }

    public override void Exit(Crop crop)
    {
        crop.HideThirstyIcon();
    }
}
