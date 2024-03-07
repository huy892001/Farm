using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionBar : MonoBehaviour
{
    [SerializeField] MissionUIItem missionItemSlotPrefab;
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject nextLevelBtn;
    [SerializeField] GameObject winDialog; 
    //[SerializeField] CropMission[] cropsMission;
    List<MissionUIItem> itemSlots = new List<MissionUIItem>();
    LevelManager levelManager = null;
    LevelData levelData;


    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void LoadMissionData(LevelData levelData)
    {
        this.levelData = levelData;
        // levelData = Resources.Load<LevelData>(levelDataPath + (levelManager.GetCurrentLevel()));
        InitializeStartSeeds();
        InitItemSlots();
    }

    private void OnEnable()
    {
    }

    private void InitializeStartSeeds()
    {
        foreach (var seed in levelData.GetInitSeeds())
        {
            Inventory.Instance.AddItem(seed.InventoryItem, seed.Quantity);
        }
    }

    void InitItemSlots()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            Destroy(itemSlots[i].gameObject);
        }
        itemSlots.Clear();
        MissionBase[] missions = levelData.GetMissions();
        for (int i = 0; i < missions.Length; i++)
        {
            foreach (var crop in missions[i].GetItems())
            {
                MissionUIItem missionUI = Instantiate(missionItemSlotPrefab, transform);
                missionUI.Uncomplete();
                missionUI.SetAvatar(crop.InventoryItem.Avatar);
                missionUI.SetCropItem(crop);
                missionUI.SetQuantity(crop.InventoryItem.GetQuantityInInventory(), crop.Quantity);
                missionUI.UpdateUI();
                itemSlots.Add(missionUI);
            }
        }
    }

    public void UpdateItemUIAndCheckIfMissionCompleted()
    {
        // Debug.Log("Update item ui and check if mission completed");
        UpdateUI();
        CheckIfMissionCompleted();
    }
    
    public void CheckIfMissionCompleted()
    {
        MissionBase[] missions = levelData.GetMissions();
        bool isCompleted = true;
        foreach (var mission in missions)
        {
            if (!mission.CheckIfCompleted())
            {
                isCompleted = false;
                break;
            }
        }
        if (isCompleted)
        {
            GameManager.Instance.Win();
        }
    }

    // event update ui
    public void UpdateUI()
    {
        foreach (var item in itemSlots)
        {
            if (item)
            {
                item.UpdateUI();
            } else
            {
                Debug.Log("Item is null");
            }
        }
    }
}
