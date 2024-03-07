using UnityEngine;
using DG.Tweening;
using System;

public class Crop : MonoBehaviour
{
    [SerializeField] CropItem cropItem;
    //[SerializeField] Inventory inventory;

    [SerializeField] BaseState normalState;
    [SerializeField] BaseState thirstyState;
    [SerializeField] BaseState ripedState;
    [SerializeField] BaseState diseasedState;

    [HideInInspector]
    public bool IsPlanted = false;
    [HideInInspector]
    public bool IsRiped = false;
    public CropFactory CropFactory;

    Field field;
    MeshRenderer meshRenderer;
    float growTimer = 0f;
    bool stopGrowing = false;
    BaseState curState = null;
    float thirstyTime, diseasedTime;
    float growTimerWhenDiseased = 0;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        ComputeTimes();
    }

    private void ComputeTimes()
    {
        curState = normalState;
        growTimer = 0f;
        thirstyTime = float.MaxValue;
        diseasedTime = float.MaxValue;
        float rdThirsty = UnityEngine.Random.Range(1f, 1000f);
        if (rdThirsty / 10f <= cropItem.GetThirstyPercent())
        {
            thirstyTime = cropItem.GetGrowthTime() * UnityEngine.Random.Range(.5f, .9f);
        }
        else
        {
            float rdDiseased = UnityEngine.Random.Range(1f, 1000f);
            if (rdDiseased / 10f <= cropItem.GetDiseasedPercent())
            {
                diseasedTime = cropItem.GetGrowthTime() * UnityEngine.Random.Range(.5f, .9f);
            }
        }
        //Debug.Log("Grown time: " + cropItem.GetGrowTime() + " thirsty time: " + thirstyTime + " diseased time: " + diseasedTime);
    }

    private void Update()
    {
        if (curState != null)
        {
            curState.Execute(this);
        }
    }

    public void Grow()
    {
        if (growTimerWhenDiseased == 0 || growTimer >= growTimerWhenDiseased)
        {
            transform.localScale += Vector3.one * (Time.deltaTime / GetGrowTime());
        }
        growTimer += Time.deltaTime;
        if (transform.localScale.x >= 1f)
        {
            growTimerWhenDiseased = 0;
            ChangeState(ripedState);
        } else if (growTimer >= thirstyTime)
        {
            ChangeState(thirstyState);
            thirstyTime = float.MaxValue;
        }
        else if (growTimer >= diseasedTime)
        {
            ChangeState(diseasedState);
            diseasedTime = float.MaxValue;
        }
    }
    public void Degrow()
    {
        if (growTimerWhenDiseased == 0) growTimerWhenDiseased = growTimer;
        if (growTimer - Time.deltaTime >= .2f * GetGrowTime())
        {
            growTimer -= Time.deltaTime;
        }
    }

    public void DisplayThirstyIcon()
    {
        field.GetCropStateUI().SetActiveWaterIcon(true);
        //field.GetCropStateUI().SetActiveBtnWatering(true);
    }

    public void HideThirstyIcon()
    {
        field.GetCropStateUI().SetActiveWaterIcon(false);
        //field.GetCropStateUI().SetActiveBtnWatering(false);
    }
    public void DisplayDiseasedIcon()
    {
        field.GetCropStateUI().SetActiveDiseaseIcon(true);
        //field.GetCropStateUI().SetActiveBtnHealing(true);
    }

    public void HideDiseasedIcon()
    {
        field.GetCropStateUI().SetActiveDiseaseIcon(false);
        //field.GetCropStateUI().SetActiveBtnHealing(false);
    }

    public void ChangeState(BaseState newState)
    {
        if (curState != null)
        {
            curState.Exit(this);
        }
        curState = newState;
        curState.Enter(this);
    }

    public void ChangeToNormalState()
    {
        ChangeState(normalState);
    }

    public void StopGrowing()
    {
        stopGrowing = true;
    }

    public void Harvest(Action onCropHarvested = null)
    {
        transform.DOMoveY(transform.position.y + 2f, .75f);
        transform.DOScale(.8f, .75f).OnComplete(() =>
        {
            Inventory.Instance.AddItem(cropItem, 1);
            onCropHarvested?.Invoke();
            Destroy(gameObject);
        });
    } 

    public BaseState GetCurrentState()
    {
        return curState;
    }

    public float GetGrowTimer()
    {
        return growTimer;
    }

    public float GetGrowTime()
    {
        return cropItem.GetGrowthTime();
    }

    public void SetField(Field fieldToSet)
    {
        field = fieldToSet;
    }

    public CropItem GetCropItem()
    {
        return cropItem;
    }
    public float GetGrowPercent()
    {
        return growTimer / GetGrowTime();
    }
}

public enum CropType
{
    WHEAT,
    CORN,
    BEET,
    PUMPKIN,
    SUNFLOWER,
    CABBAGE,
    TOMATO
}

