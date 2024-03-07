using UnityEngine;

public class CookingDialog : Dialog 
{
    [SerializeField] CakeUIItem[] cakeUIItems;
    [SerializeField] GameObject cakesPanel; 
    [SerializeField] CookingPanel cookingPanel;

    CakeItem currentCake = null;
    float remainingTime = 0f;

    protected override void Start()
    {
        base.Start();
        if (currentCake == null)
        {
            ShowCakeMenu();
        }
        else
        {
            ShowCookingPanel(currentCake, remainingTime);
        }
    }
    public void SetCakeItem(CakeItem cake, float remainTimeParam)
    {
        currentCake = cake;
        remainingTime = remainTimeParam;
    }
    public void StartCooking(CakeItem cakeToCook, float remainTime)
    {
        ShowCookingPanel(cakeToCook, remainTime);
    }
    public void ShowCakeMenu()
    {
        Debug.LogError("Show cake menu");
        title.text = "CAKES SHOP";
        currentCake = null;
        remainingTime = 0f;
        if (FindObjectOfType<CakeShop>()) {
            FindObjectOfType<CakeShop>().ResetCook();
        }
        cookingPanel.gameObject.SetActive(false);
        cakesPanel.gameObject.SetActive(true);
        for (int i = 0; i < cakeUIItems.Length; i++)
        {
            cakeUIItems[i].UpdateUI();
        }
    }
    public void ShowCookingPanel(CakeItem cake, float remainTime)
    {
        Debug.LogError("Show cooking panelllllllllllllllllllllll: " + cake.name);
        title.text = "COOKING";
        cakesPanel.gameObject.SetActive(false);
        cookingPanel.gameObject.SetActive(true);
        cookingPanel.SetCakeItem(cake, remainTime);
    }

    public override void Close()
    {
        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO)
        {
            playerGO.GetComponent<PlayerMovement>().SetMovingState(true);
        }
        if (cookingPanel.gameObject.activeInHierarchy)
        {
            FindObjectOfType<CakeShop>().SetCakeItem(cookingPanel.GetCurrentCakeItem(), cookingPanel.GetCookTimer());
        }
        base.Close();
    }
}
