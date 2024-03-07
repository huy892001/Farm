using UnityEngine;
using UnityEngine.UI;

public class CropStateUI : MonoBehaviour
{
    [SerializeField] GameObject waterIcon, diseaseIcon;
    [SerializeField] Image percentImage;
    [SerializeField] Sprite pinkBarSprite, greenBarSprite;

    private void Start()
    {
        percentImage.sprite = pinkBarSprite;
        percentImage.type = Image.Type.Filled;
        ResetUI();
    }

    public void SetFillOfTimerImg(float percent)
    {
        percentImage.fillAmount = percent;
        if (Mathf.Approximately(percentImage.fillAmount, 1f)) {
            percentImage.sprite = greenBarSprite;
        }
    }
    public void SetActiveTimerImg(bool state)
    {
        // Debug.Log("Set active timer img");
        percentImage.gameObject.SetActive(state);
    }
    public void SetActiveWaterIcon(bool state)
    {
        waterIcon.SetActive(state);
    }
    public void SetActiveDiseaseIcon(bool state)
    {
        diseaseIcon.SetActive(state);
    }

    public void ResetUI()
    {
        SetActiveTimerImg(false);
        SetActiveWaterIcon(false);
        SetActiveDiseaseIcon(false);
    }
}
