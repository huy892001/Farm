using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingPanel : MonoBehaviour
{
    [SerializeField] RectTransform avatarRect;
    [SerializeField] Image avatar;
    [SerializeField] TextMeshProUGUI cookTimeTxt;
    [SerializeField] int initPosition, donePosition;

    CakeItem cakeItem;
    WaitForSeconds waitOneSecond = new WaitForSeconds(1);
    float cookTimer;
    bool isCookingDone = false;
    bool hasTook = false;


    private void OnEnable()
    {
        hasTook = false;
        isCookingDone = false;
    }

    public void SetCakeItem(CakeItem item, float remainTime = 0)
    {
        cakeItem = item;
        avatarRect.SetYAnchoredPosition(initPosition);
        avatar.sprite = cakeItem.Avatar;
        cookTimeTxt.transform.parent.gameObject.SetActive(true);
        cookTimeTxt.text = remainTime.ToString(); //cakeItem.GetCookTime().ToString();
        cookTimer = remainTime;
        if (cookTimer <= 0f)
        {
            CookDone();
        } else
        {
            StartCoroutine(CountdownTime());
        }
    }

    IEnumerator CountdownTime()
    {
        while (cookTimer > 0)
        {
            yield return null;
            cookTimer -= Time.deltaTime;
            cookTimeTxt.text = Mathf.RoundToInt(cookTimer).ToString();
            if (cookTimer <= 0)
            {
                CookDone();
                break;
            } 
        }
    } 

    public float GetCookTimer()
    {
        return cookTimer;
    }

    public CakeItem GetCurrentCakeItem()
    {
        return cakeItem;
    }
    
    void CookDone()
    {
        // avatar.GetComponent<RectTransform>().SetYAnchoredPosition(donePosition);
        avatarRect.SetYAnchoredPosition(donePosition);
        cookTimeTxt.transform.parent.gameObject.SetActive(false);
        // avatarRect.gameObject.SetActive(false);
        // getBtn.interactable = true;
        isCookingDone = true;
    }

    public void TakeCake()
    {
        if (isCookingDone) {
            if (hasTook) return;
            hasTook = true;
            RectTransform pos = avatar.GetComponent<RectTransform>();
            FloatingUIItemController.Instance.ShowWithAnchoredPosition(cakeItem.Avatar, 1, pos.anchoredPosition
                , pos.anchoredPosition + Vector2.up * 150,
                () =>
                {
                    Inventory.Instance.AddItem(cakeItem);
                    FindObjectOfType<CookingDialog>().ShowCakeMenu();
                    cakeItem = null;
                });
        } else {
            ToastManager.Instance.ShowNotifyRect("NOT DONE", avatarRect.anchoredPosition);
        }
    }
    //IEnumerator COTakeCake()
    //{
    //    Inventory.Instance.AddItem(cakeItem);
    //    FindObjectOfType<CookingDialog>().ShowCakeMenu();
    //    onItemQuantityChanged.NotifyAll();
    //    cakeItem = null;
    //}
}
