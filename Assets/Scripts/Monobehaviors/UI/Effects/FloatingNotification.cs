using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingNotification : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtContent;

    public void SetContent(string content)
    {
        txtContent.text = content;
    }
}
