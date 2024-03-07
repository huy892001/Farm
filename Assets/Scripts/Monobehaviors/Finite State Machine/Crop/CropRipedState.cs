using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crop Riped State", menuName = "Crop States/Riped")]
public class CropRipedState : BaseState
{
    public override void Enter(Crop crop)
    {
        crop.IsRiped = true;
    }
    public override void Execute(Crop crop)
    {
    }
    public override void Exit(Crop crop)
    {
    }
}
