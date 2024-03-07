using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedItemBtn : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] Image avatarImg;
    [SerializeField] Image border;

    
    SeedBarManager seedBarManager;
    public Item itemData;
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
        if (seedBarManager != null)
        {
            seedBarManager.SetChoosedItem(this);
        } else
        {
            Debug.LogError("Toolbar manager is null: " + name);
        }
    } 
    public void SetItemData(Item item)
    {
        itemData = item;
    }
    public Item GetItemData()
    {
        return itemData;
    }
    public void SetSpriteAvatar(Sprite avatarSprite)
    {
        avatarImg.sprite = avatarSprite;
        //avatarImg.SetNativeSize();
    }
    public void SetQuantity(int number)
    {
        quantityText.text = number.ToString();
    }
    public int GetQuantity()
    {
        //Debug.LogError("Current quantity: " + quantityText.text);
        return int.Parse(quantityText.text);
    }
    public void SetActiveBorder(bool state)
    {
        border.gameObject.SetActive(state);
    }
    public void SetSeedBarManager(SeedBarManager seedBarManager)
    {
        this.seedBarManager = seedBarManager;
    }
}
