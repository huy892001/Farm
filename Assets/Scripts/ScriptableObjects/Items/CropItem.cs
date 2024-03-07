using UnityEngine;

[CreateAssetMenu(menuName = "Items/Crop item")]
public class CropItem : Item, IExchangeable
{
    [SerializeField] int grownDuration;
    [SerializeField, Range(0, 100)] float diseasedPercent, thirstyPercent;
    [SerializeField] CropType cropType;

    public CropType GetCropType()
    {
        return cropType;
    }
    public float GetThirstyPercent()
    {
        return thirstyPercent; 
    }
    public float GetDiseasedPercent()
    {
        return diseasedPercent;
    }
    public int GetGrowthTime()
    {
        return grownDuration;
    }
}
