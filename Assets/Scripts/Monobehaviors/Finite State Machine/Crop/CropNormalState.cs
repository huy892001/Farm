using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crop Normal State", menuName = "Crop States/Normal")]
public class CropNormalState : BaseState
{
    public override void Enter(Crop crop)
    {
    }

    public override void Execute(Crop crop)
    {
        crop.Grow();
    }

    public override void Exit(Crop crop)
    {
    }
}
