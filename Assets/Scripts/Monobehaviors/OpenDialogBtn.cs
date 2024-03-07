using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenDialogBtn : MonoBehaviour
{
    [SerializeField] DialogType dialogType;

    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void OnEnable()
    {
        button.onClick.AddListener(ShowDialog);
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(ShowDialog);
    }
    public void ShowDialog()
    {
        DialogController.Instance.ShowDialog(dialogType);
    }
}
