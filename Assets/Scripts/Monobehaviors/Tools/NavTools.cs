using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
  
#if UNITY_EDITOR
public class NavTools : MonoBehaviour
{
    [MenuItem("Tools/Remove all data")]
    public static void RemoveAllData()
    {
        ZPlayerPrefs.DeleteAll();
        Debug.Log("Delete all data successfull");
    }
}

#endif
