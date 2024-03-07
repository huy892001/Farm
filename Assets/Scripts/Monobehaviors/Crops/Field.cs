using DG.Tweening;
using UnityEngine;

public class Field : MonoBehaviour
{
    // [SerializeField] GrownTimeDisplay grownTimeDisplay;
    [SerializeField] CropStateUI cropStateUI;
    [SerializeField] GameObject diedCrop;
    [SerializeField] Color normalColor, havestedCropColor, highlightColor, ripedColor;
    [SerializeField] Renderer curRenderer;
    
    Crop currentCrop = null;
    FieldState curState;


    public CropStateUI GetCropStateUI()
    {
        return cropStateUI; 
    }

    public bool HasCrop
    {
        get => curState == FieldState.HAS_CROP;
        private set { }
    }
    public bool HasDiedCrop
    {
        get => curState == FieldState.HAS_DIED_CROP; 
    }
    public FieldState State
    {
        get => curState;
        set => curState = value;
    }

    public void ResetState()
    {
        currentCrop = null;
        cropStateUI.ResetUI();
    }

    public void SetCrop(Crop cropToSet)
    {
        curState = FieldState.HAS_CROP;
        currentCrop = cropToSet;
    }

    public Crop GetCurrentCrop()
    {
        return currentCrop;
    }
    public Renderer GetRenderer()
    {
        return curRenderer;
    }

    public void Harvest()
    {
        diedCrop.gameObject.SetActive(true);
        diedCrop.transform.localScale = Vector3.zero;
        diedCrop.transform.DOScale(1.0f, .5f);
        curState = FieldState.HAS_DIED_CROP;
        currentCrop.Harvest();
        ResetState();
        SetToNormalColor();
    }

    public void SetToNormalColor()
    {
        if (curState == FieldState.HAS_DIED_CROP)
        {
            curRenderer.material.color = havestedCropColor;
        } else
        {
            curRenderer.material.color = normalColor;
        }
    }
    public void ChangeToHighlightColor()
    {
        curRenderer.material.color = highlightColor;
    }
    public void ChangeToRipedColor()
    {
        curRenderer.material.color = ripedColor;
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.LogError("Trigger stay");
        if (other.tag == "Player" && HasCrop)
        {
            cropStateUI.SetActiveTimerImg(true);
            cropStateUI.SetFillOfTimerImg(currentCrop.GetGrowPercent());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.LogError("Trigger exit");
        if (other.tag == "Player")
        {
            cropStateUI.SetActiveTimerImg(false);
        }
    }
    public void Hoeing()
    {
        //Debug.LogError("Hoeinggggggggggg");
        curState = FieldState.NORMAL;
        diedCrop.transform.DOScale(1.2f, .3f)
            .OnComplete(() =>
            {
                diedCrop.transform.DOScale(1.0f, .15f)
                .OnComplete(() =>
                {
                    diedCrop.gameObject.SetActive(false);
                });
            });
    }
    public void Watering()
    {
        if (currentCrop != null)
        {
            currentCrop.ChangeToNormalState();
        }
    }
    public void Healing()
    {
        if (currentCrop != null)
        {
            currentCrop.ChangeToNormalState();
        }
    }
    public enum FieldState
    {
        NORMAL,
        HAS_CROP,
        HAS_DIED_CROP
    }
}
