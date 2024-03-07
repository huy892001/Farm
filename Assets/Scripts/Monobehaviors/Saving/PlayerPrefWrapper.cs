using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefWrapper 
{
    const string currentLevel = "CURRENT_LEVEL";

    public static int CurrentLevel
    {
        get
        {
            return ZPlayerPrefs.GetInt(currentLevel, 1);
        }
        set
        {
            ZPlayerPrefs.SetInt(currentLevel, value);
        }
    }
}
