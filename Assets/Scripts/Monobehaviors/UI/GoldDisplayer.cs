using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] Transform goldIconTrans;

    int curGold = 0;

    public Transform GetGoldIconTrans()
    {
        return goldIconTrans;
    }

    public void SetGold(int amount)
    {
        curGold = amount;
        goldText.text = curGold.ToString();
    }

    public void Show()
    {
        GetComponent<Canvas>().overrideSorting = true; 
    }

    public void Hide()
    {
        GetComponent<Canvas>().overrideSorting = false; 
    }
}
