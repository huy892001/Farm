using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainScene : MonoBehaviour
{
    public void LoadMainScene()
    {
        Debug.Log("Load main scene");
        SceneManager.LoadScene(1);
    }
}
