using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoadNextLevel : MonoBehaviour
{
    public void LoadNextLevel()
    {
        FindObjectOfType<LevelManager>().LoadNextLevel();
    }
}
