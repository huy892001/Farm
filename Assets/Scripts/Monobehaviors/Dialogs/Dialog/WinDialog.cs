using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class WinDialog : Dialog 
{
    [SerializeField] TextMeshProUGUI levelTxt, completedDurationTxt;

    protected override void Start() {
        base.Start();
        levelTxt.text = "LEVEL " + LevelManager.Instance.GetCurrentLevel().ToString();
        TimeSpan completeTimeSpan = FindObjectOfType<LevelTimer>().GetCompletedTime();
        completedDurationTxt.text = string.Format("{0:D2}:{1:D2}", completeTimeSpan.Minutes, completeTimeSpan.Seconds);
    }
    public void NextLevel()
    {
        Close();
        FindObjectOfType<LevelManager>().LoadNextLevel();
    }
    public void ReloadCurrentLevel() {
        Close();
        StartCoroutine(LevelManager.Instance.LoadCurrentLevel());
    }
}
