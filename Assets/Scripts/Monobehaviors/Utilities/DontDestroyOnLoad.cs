using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsOfType<DontDestroyOnLoad>().Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }
}
