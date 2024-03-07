using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_", menuName = "Level/Game level")]
public class LevelData : ScriptableObject 
{
    [SerializeField] int initGold = 0;
    [SerializeField] ItemHolder[] initSeeds;
    [SerializeField] MissionBase[] missions;
    [SerializeField] float time;

    public float GetTime()
    {
        return time;
    }
    public bool HasTime()
    {
        return time > 0f;
    }
    public int GetInitCoin()
    {
        return initGold;
    }
    public MissionBase[] GetMissions()
    {
        return missions;
    }
    public ItemHolder[] GetInitSeeds()
    {
        return initSeeds;
    }
    public bool CheckIfLevelCompleted()
    {
        foreach (MissionBase mission in missions)
        {
            if (!mission.CheckIfCompleted())
            {
                return false;
            }
        }
        return true;
    }
}
