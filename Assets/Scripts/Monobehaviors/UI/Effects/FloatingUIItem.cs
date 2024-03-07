using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloatingUIItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numText;
    [SerializeField] Image itemImage;
    [SerializeField] Color addColor, subColor;

    public void SetData(Sprite itemSprite, int num)
    {
        itemImage.sprite = itemSprite;
        if (num < 0)
        {
            numText.text = "-" + Mathf.Abs(num).ToString();
            numText.color = subColor;
        } else
        {
            numText.text = "+" + num.ToString();
            numText.color = addColor;
        }
    }
}
