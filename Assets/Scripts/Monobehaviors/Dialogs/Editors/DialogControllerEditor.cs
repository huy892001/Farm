using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(DialogController))]
[CanEditMultipleObjects]
public class DialogControllerEditor : BaseInspector
{
    public SerializedProperty baseDialogs;
    private void OnEnable()
    {
        baseDialogs = serializedObject.FindProperty("baseDialogs");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        ShowArrayProperty(baseDialogs, typeof(DialogType), "Dialogs");
        serializedObject.ApplyModifiedProperties();
    }
}
#endif