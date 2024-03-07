using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

#if UNITY_EDITOR
[CustomEditor(typeof(CropFactory))]
public class CropFactoryEditor : Editor
{
    const string cropPrefabsProName = "cropPrefabs";
    SerializedProperty cropPrefabsProperty;
    string[] cropTypeNames;

    private void OnEnable()
    {
        cropPrefabsProperty = serializedObject.FindProperty(cropPrefabsProName);
        cropTypeNames = Enum.GetNames(typeof(CropType));
        cropPrefabsProperty.serializedObject.Update();
        cropPrefabsProperty.arraySize = cropTypeNames.Length;
        cropPrefabsProperty.serializedObject.ApplyModifiedProperties();
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        cropTypeNames = Enum.GetNames(typeof(CropType));
        for (int i = 0; i < cropTypeNames.Length; i++)
        {
            EditorGUILayout.PropertyField(cropPrefabsProperty.GetArrayElementAtIndex(i), new GUIContent(cropTypeNames[i]));
        }
        serializedObject.ApplyModifiedProperties();
    }
}

#endif