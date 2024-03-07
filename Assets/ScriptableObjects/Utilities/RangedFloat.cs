using System;
using UnityEngine;

[Serializable]
public struct RangedFloat
{
	public float minValue;
	public float maxValue;
	public float GetRandomValueInRange()
    {
		return UnityEngine.Random.Range(minValue, maxValue);
    }
}