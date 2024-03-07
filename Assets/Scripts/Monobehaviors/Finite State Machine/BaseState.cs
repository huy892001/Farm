using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : ScriptableObject  
{
    public abstract void Enter(Crop crop);
    public abstract void Execute(Crop crop);
    public abstract void Exit(Crop crop);
}
