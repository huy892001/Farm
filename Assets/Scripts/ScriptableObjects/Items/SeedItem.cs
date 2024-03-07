using UnityEngine;

[CreateAssetMenu(menuName = "Items/Seed item")]
public class SeedItem : Item, IExchangeable, IBuyable 
{
    [SerializeField] int unlockLevel;
    [SerializeField] CropItem cropItem;


    public CropType GetCropType()
    {
        return cropItem.GetCropType();
    }

    public int GetUnlockLevel()
    {
        return unlockLevel;
    }
    public CropItem GetCropItem()
    {
        return cropItem;
    }
}
