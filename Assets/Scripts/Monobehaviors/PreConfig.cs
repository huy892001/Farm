using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreConfig : MonoBehaviour
{
    [SerializeField] bool isDebug = false;
    [SerializeField] int currentLevel = 1;

    private void Start() {
        if (isDebug) {
            PlayerPrefWrapper.CurrentLevel = currentLevel;
        }
    }
}
