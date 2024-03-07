using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingDialog : Dialog 
{
    public void GoHome() {
        this.Close();
        SceneManager.LoadSceneAsync("home");
    }
}
