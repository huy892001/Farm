using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToolItemUI : MonoBehaviour
{
    [SerializeField] Image avatarImg;
    [SerializeField] Image border;
    [SerializeField] ToolType type;

    public enum ToolType
    {
        NONE, HOE, WATERING, MEDICINE, PUNCH 
    }

    public ToolBarManager toolBarManager;
    Button btnComp;

    private void Awake()
    {
        btnComp = GetComponent<Button>();
    }

    private void Start()
    {
        border.gameObject.SetActive(false);
        btnComp.onClick.RemoveAllListeners();
        btnComp.onClick.AddListener(OnBtnClick);
    }

    public void OnBtnClick()
    {
        if (toolBarManager != null)
        {
            toolBarManager.SetChoosedItem(this);
        } else
        {
            Debug.LogError("Toolbar manager is null: " + name);
        }
    } 

    public void SetSpriteAvatar(Sprite avatarSprite)
    {
        avatarImg.sprite = avatarSprite;
        //avatarImg.SetNativeSize();
    }
    public void SetActiveBorder(bool state)
    {
        border.gameObject.SetActive(state);
    }
    public void SetToolBarManager(ToolBarManager toolBarManager)
    {
        this.toolBarManager = toolBarManager;
    }
    public ToolType GetToolType()
    {
        return type;
    }
}
