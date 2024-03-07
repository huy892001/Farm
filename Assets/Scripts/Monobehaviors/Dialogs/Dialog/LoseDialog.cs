using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseDialog : Dialog 
{
    public void PlayAgain()
    {
        Close();
        StartCoroutine(FindObjectOfType<LevelManager>().LoadCurrentLevel());
    }
    public void Home() {
        SceneManager.LoadScene(0);
    }
}
