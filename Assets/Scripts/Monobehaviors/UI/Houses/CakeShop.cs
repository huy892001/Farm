using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CakeShop : MonoBehaviour
{
    [SerializeField] GameObject cakeTimeHolder;
    [SerializeField] Image cakeImage;
    [SerializeField] TextMeshProUGUI cookTimerTxt;
    [SerializeField] GameObject cookDoneIcon;
    [SerializeField] GameObject canvas;

    CakeItem cakeItem = null;
    float cookTimer = 0f;

    WaitForSeconds waitOneSecond = new WaitForSeconds(1f);
    Coroutine coCountDownTime = null;

    private void Start()
    {
        // cakeHolder.SetActive(false);
        canvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement)
        {
            playerMovement.SetMovingState(false);
            CookingDialog cookingDialog = DialogController.Instance.ShowDialog(DialogType.COOKING) as CookingDialog; 
            if (cakeItem)
            {
                Debug.LogError("Cake item not null in cake shoppppppppppppppppppp");
                cookingDialog.SetCakeItem(cakeItem, cookTimer);
            }
        }
    }

    public void ResetCook()
    {
        canvas.SetActive(false);
        cakeItem = null;
        cookTimer = 0f;
        cookDoneIcon.SetActive(false);
        // cakeHolder.SetActive(false);
    }

    public void SetCakeItem(CakeItem cake, float remainTime)
    {
        Debug.LogError("Set cake itemmmmmmmmmmm: " + remainTime);
        cakeTimeHolder.SetActive(true);
        canvas.SetActive(true);
        cakeItem = cake;
        cakeImage.sprite = cake.Avatar;
        cookTimerTxt.text = (Mathf.RoundToInt(remainTime)).ToString();
        cookTimer = remainTime;
        cookDoneIcon.SetActive(false);
        if (cookTimer <= 0f)
        {
            CookDone();
        } else
        {
            if (coCountDownTime != null) {
                StopCoroutine(coCountDownTime);
            }
            coCountDownTime = StartCoroutine(CountdownTime());
        }
    }

    IEnumerator CountdownTime()
    {
        while (cookTimer > 0)
        {
            yield return null;
            cookTimer -= Time.deltaTime;
            cookTimerTxt.text = Mathf.RoundToInt(cookTimer).ToString();
            if (cookTimer <= 0f)
            {
                CookDone();
                break;
            } 
        }
    } 

    void CookDone()
    {
        Debug.Log("Cook done");
        cookDoneIcon.SetActive(true);
        cakeTimeHolder.SetActive(false);
    } 
}
