using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTextDisplay : MonoBehaviour
{
    TextMeshProUGUI levelTxt;
    private void Awake() {
        levelTxt = GetComponent<TextMeshProUGUI>();
    }
    public void SetLevel(int level) {
        levelTxt.text = "LEVEL " + level;
    }
}
