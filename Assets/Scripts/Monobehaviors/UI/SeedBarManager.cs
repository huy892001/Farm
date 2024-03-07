using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBarManager : MonoBehaviour
{
    [SerializeField] SeedItemBtn seedItemBtnPrefab;

    List<SeedItemBtn> seedUIs = new List<SeedItemBtn>();
    SeedItemBtn currentChoosedSeed = null;
    PlayerController playerController;
    CanvasGroup canvasGroup;
    bool isShow = true;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("Can not find player controller");
        }
    }
    public void Show()
    {
        if (isShow) return;
        UpdateSeedQuantity();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        isShow = true;
    }
    public void Hide()
    {
        if (!isShow) return;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        isShow = false;
    }
    public bool IsShow()
    {
        return isShow;
    }

    public void DisplaySeedItems()
    {
        Inventory inventory = Inventory.Instance;
        for (int i = 0; i < seedUIs.Count; i++)
        {
            Destroy(seedUIs[i].gameObject);
        }
        seedUIs.Clear();
        bool choosedSeedAlive = false;
        Debug.LogError("Inventory count: " + inventory.items.Count);
        for (int i = 0; i < inventory.items.Count; i++)
        {
            SeedItem seedItem = inventory.items[i].InventoryItem as SeedItem;
            if (seedItem)
            {
                Debug.LogError("Is seed item");
                SeedItemBtn seedItemBtn = Instantiate(seedItemBtnPrefab);
                seedItemBtn.transform.SetParent(transform);
                seedItemBtn.transform.localScale = Vector3.one;
                seedItemBtn.SetSpriteAvatar(seedItem.Avatar);
                seedItemBtn.SetQuantity(Inventory.Instance.GetQuantity(seedItem));
                seedItemBtn.SetItemData(seedItem);
                seedItemBtn.SetSeedBarManager(this);
                seedUIs.Add(seedItemBtn);
                if (currentChoosedSeed != null && seedItem.Id == currentChoosedSeed.GetItemData().Id && currentChoosedSeed.GetQuantity() > 0)
                {
                    choosedSeedAlive = true;
                }
            } 
        }
        if (!choosedSeedAlive)
        {
            currentChoosedSeed = null;
        }
        if (currentChoosedSeed != null)
        {
            currentChoosedSeed.SetActiveBorder(true);
            playerController.SetSeed(currentChoosedSeed);
            //Debug.Log("Set current seed border to true");
        } else
        {
            Debug.Log("Current choosed seed is null");
        }
        Debug.Log("Choosed seed alive: " + choosedSeedAlive);
    }

    public void UpdateSeedQuantity()
    {
        for (int i = seedUIs.Count - 1; i >= 0; i--)
        {
            if (!Inventory.Instance.HasItem(seedUIs[i].GetItemData()))
            {
                Debug.LogError("Inventory dont has seed item");
                if (currentChoosedSeed != null && currentChoosedSeed.GetItemData().Id == seedUIs[i].GetItemData().Id)
                {
                    currentChoosedSeed = null;
                }
                Destroy(seedUIs[i].gameObject);
                seedUIs.RemoveAt(i);
            } else
            {
                int curQuantity = Mathf.Max(0, Inventory.Instance.GetQuantity(seedUIs[i].GetItemData()));
                seedUIs[i].SetQuantity(curQuantity);
                if (curQuantity <= 0)
                {
                    seedUIs[i].gameObject.SetActive(false);
                } else
                {
                    seedUIs[i].gameObject.SetActive(true);
                }
            }
        }
        List<ItemHolder> items = Inventory.Instance.items;
        for (int i = 0; i < items.Count; i++)
        {
            SeedItem seedItem = items[i].InventoryItem as SeedItem;
            if (seedItem && items[i].Quantity > 0)
            {
                if (!HasSeedItem(items[i].InventoryItem))
                {
                    Debug.LogError("not has seed item when update seed bar");
                    SeedItemBtn seedItemBtn = Instantiate(seedItemBtnPrefab);
                    seedItemBtn.transform.SetParent(transform);
                    seedItemBtn.transform.localScale = Vector3.one;
                    seedItemBtn.SetSpriteAvatar(items[i].InventoryItem.Avatar);
                    seedItemBtn.SetQuantity(Inventory.Instance.GetQuantity(items[i].InventoryItem));
                    seedItemBtn.SetItemData(items[i].InventoryItem);
                    seedItemBtn.SetSeedBarManager(this);
                    seedUIs.Add(seedItemBtn);
                }
           }
        }
    }
    public List<SeedItemBtn> GetSeedItemBtns()
    {
        return seedUIs; 
    }
    public bool HasSeedItem(Item item)
    {
        //Debug.Log("Item id: " + item.Id);
        foreach (var seedUI in seedUIs)
        {
            //Debug.LogError("Seed id: " + seedUI.GetItemData().Id);
            if (seedUI.GetItemData().Id == item.Id)
            {
                return true;
            }
        }
        return false;
    }
    public int GetSeedItemCount()
    {
        int cnt = 0;
        foreach (var seed in seedUIs)
        {
            if (seed.gameObject.activeSelf) cnt++;
        }
        //Debug.LogError("Seed item count: " + cnt);
        return cnt;
    } 
    public SeedItemBtn GetFirstSeedItemBtn()
    {
        foreach (var seed in seedUIs)
        {
            if (seed.GetQuantity() > 0)
            {
                return seed;
            }
        }
        return null;
    }
    public void SetChoosedItem(SeedItemBtn seedItemBtn)
    {
        if (currentChoosedSeed != null)
        {
            currentChoosedSeed.SetActiveBorder(false);
            Debug.Log("[SeedBarManager] Unactivate current choosed seed");
        } else
        {
            Debug.Log("[SeedBarManager] Currnet choosed seed is null");
        }
        currentChoosedSeed = seedItemBtn;
        if (currentChoosedSeed)
        {
            currentChoosedSeed.SetActiveBorder(true);
        } else
        {
            Debug.Log("[SeedBarManager] seed item btn is null");
        }
        playerController.SetSeed(seedItemBtn);
    }
}
