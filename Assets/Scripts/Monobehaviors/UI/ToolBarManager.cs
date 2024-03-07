using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarManager : MonoBehaviour
{
    [SerializeField] ToolItemUI toolItemUIPrefab;
    [SerializeField] ToolItemUI waterCan;
    [SerializeField] ToolItemUI hoe;
    [SerializeField] ToolItemUI punch;

    CanvasGroup canvasGroup;
    List<ToolItemUI> seedUIs = new List<ToolItemUI>();
    ToolItemUI choosedItem = null;
    PlayerController playerController;
    bool isShowing = true;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        GameObject playerGameObj = GameObject.FindGameObjectWithTag("Player");
        if (playerGameObj != null)
        {
            playerController = playerGameObj.GetComponent<PlayerController>(); 
        } else
        {
            Debug.LogError("Cannot find player game object");
        }
    }
    public ToolItemUI GetFirstToolItem()
    {
        return hoe;
    }

    public void SetChoosedItem(ToolItemUI item)
    {
        if (choosedItem != null)
        {
            choosedItem.SetActiveBorder(false);
        }
        choosedItem = item;
        choosedItem.SetActiveBorder(true);
        playerController.EquipItem(item);
    }
    public void Show()
    {
        if (isShowing) return;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        isShowing = true;
    }
    public void Hide()
    {
        if (!isShowing) return;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        isShowing = false;
    }
    public bool IsShow()
    {
        return isShowing;
    }
}
