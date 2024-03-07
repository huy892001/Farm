using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crop Diseased State", menuName = "Crop States/Diseased")]
public class CropDiseasedState : BaseState
{
    public override void Enter(Crop crop)
    {
        crop.DisplayDiseasedIcon();
    }

    public override void Execute(Crop crop)
    {
        crop.Degrow();
    }

    public override void Exit(Crop crop)
    {
        crop.HideDiseasedIcon();
    }
}
