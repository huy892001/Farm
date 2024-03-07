using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public abstract class EditorWithSubEditor<TEditor, TTarget> : Editor
    where TEditor : Editor 
    where TTarget : MonoBehaviour
{
    protected TEditor[] subEditors;


    protected void CheckAndCreateSubEditors(TTarget[] subEditorsTarget)
    {
        if (subEditors != null && subEditors.Length == subEditorsTarget.Length) return;
        CleanupSubEditors();
        subEditors = new TEditor[subEditorsTarget.Length];
        for (int i = 0; i < subEditorsTarget.Length; i++)
        {
            subEditors[i] = CreateEditor(subEditorsTarget[i]) as TEditor;
            SetupEditor(subEditors[i]);
        }
    }

    protected abstract void SetupEditor(TEditor editor);

    private void OnDisable()
    {
        CleanupSubEditors();
    }

    void CleanupSubEditors()
    {
        if (subEditors == null) return;
        for (int i = 0; i < subEditors.Length; i++)
        {
            DestroyImmediate(subEditors[i]);
        }
        subEditors = null;
    }
}

#endif