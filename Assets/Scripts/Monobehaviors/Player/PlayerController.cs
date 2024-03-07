using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform rightHandTrans;
    [SerializeField] GameObject hoe;
    [SerializeField] GameObject wateringCan;
    [SerializeField] GameObject medicineCan;
    [SerializeField] Item hoeUse, medicineUse, waterUse;
    Animator m_animator;
    int hoeParam = Animator.StringToHash("Hoe");
    int wateringParam = Animator.StringToHash("Watering");
    int seedParam = Animator.StringToHash("Seed");
    int healingParam = Animator.StringToHash("Healing");
    int m_animSpeedParam = Animator.StringToHash("MoveSpeed");
    int feedingParam = Animator.StringToHash("Feeding");

    ToolItemUI.ToolType curToolType = ToolItemUI.ToolType.NONE;
    PlayerPlanting playerPlantingComp;

    GameObject currentToolItemGameObj = null;
    ToolBarManager toolBarManager;
    SeedBarManager seedBarManager;

    ToolItemUI toolItemBtn = null;
    SeedItemBtn seedItemBtn = null;
    Field currentField;
    SeedItem seedItem = null;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        playerPlantingComp = GetComponent<PlayerPlanting>();
        toolBarManager = FindObjectOfType<ToolBarManager>();
        seedBarManager = FindObjectOfType<SeedBarManager>();
    }

    private void Start()
    {
        wateringCan.SetActive(false);
        hoe.SetActive(false);
        medicineCan.SetActive(false);
        toolBarManager.Hide();
        seedBarManager.Hide();
    }
    private void Update()
    {
        currentField = playerPlantingComp.GetCurrentInteractableField();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Seeding();
        }
        if (currentField == null)
        {
            seedBarManager.Hide();
            toolBarManager.Hide();
            if (currentToolItemGameObj)
            {
                currentToolItemGameObj.SetActive(false);
                //currentToolItemGameObj = null;
            }
        }
    }

    public void Seeding()
    {
        if (currentField != null)
        {
            ResetAllTriggers();
            if (currentField != null)
            {
                if (currentField.HasCrop)
                {
                    seedBarManager.Hide();
                    HelpingTree();
                } else if (currentField.HasDiedCrop)
                {
                    seedBarManager.Hide();
                    if (CheckAndDisplayToolBar())
                    {
                        if (toolItemBtn)
                        {
                            if (toolItemBtn.GetToolType() == ToolItemUI.ToolType.HOE)
                            {
                                //transform.LookAt(currentField.transform.position);
                                SetPlayerLookView();
                                m_animator.SetTrigger(hoeParam);
                            } else
                            {
                                ToastManager.Instance.ShowNotify("INVALID TOOL", currentField.transform.position + Vector3.up);
                            }
                        } else
                        {
                            toolBarManager.Hide();
                        }
                    }
                }
                else
                {
                    if (currentToolItemGameObj) currentToolItemGameObj.SetActive(false);
                    toolBarManager.Hide();
                    // Debug.LogError("Show seeds");
                    if (!seedBarManager.IsShow())
                    {
                        seedBarManager.UpdateSeedQuantity();
                        // Debug.LogError("inside show seeds: " + seedBarManager.GetSeedItemCount());
                        if (seedBarManager.GetSeedItemCount() > 0)
                        {
                            seedBarManager.Show();
                            if (seedItemBtn == null)
                            {
                                seedBarManager.SetChoosedItem(seedBarManager.GetFirstSeedItemBtn());
                                StartCoroutine(DelayActivateSeedBorder());
                            }
                        } else
                        {
                            ToastManager.Instance.ShowNotify("OUT OF SEED", currentField.transform.position, 150, .75f);
                        }
                    }
                    else
                    {
                        if (seedItemBtn && seedItemBtn.GetQuantity() > 0)
                        {
                            seedItem = seedItemBtn.GetItemData() as SeedItem;
                            if (seedItem != null && !IsWorking())
                            {
                                //transform.LookAt(currentField.transform.position);
                                SetPlayerLookView();
                                m_animator.SetTrigger(seedParam);
                            }
                            else
                            {
                                seedBarManager.Hide();
                            }
                        }
                    }
                }
            }
        }
        else if (currentField == null)
        {
            seedBarManager.Hide();
            toolBarManager.Hide();
            //seedBarManager.gameObject.SetActive(false);
            //toolBarManager.gameObject.SetActive(false);
        }
    }

    public void SetPlayerLookView()
    {
        if (currentField)
        {
            transform.LookAt(currentField.transform.position);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
    }

    public IEnumerator DelayActivateSeedBorder()
    {
        yield return null;//new WaitForSeconds(1);
        if (seedItemBtn)
        {
            seedItemBtn.SetActiveBorder(true);
        }
    }
    public IEnumerator DelayActivateToolBorder()
    {
        yield return null;//new WaitForSeconds(1);
        if (toolItemBtn)
        {
            toolItemBtn.SetActiveBorder(true);
        }
    }

    public void DeactivateTool()
    {
        // Debug.LogError("Deactive tool");
        if (currentToolItemGameObj)
        {
            currentToolItemGameObj.SetActive(false);//.SetActiveBorder(false);
        }
    }

    private void HelpingTree()
    {
        Crop currentCrop = currentField.GetCurrentCrop();
        BaseState cropState = currentCrop.GetCurrentState();
        if (cropState as CropRipedState)
        {
            //toolBarManager.gameObject.SetActive(false);
            toolBarManager.Hide();
            currentField.Harvest();
            return;
        }
        if (CheckAndDisplayToolBar())
        {        
            if (cropState as CropNormalState)
            {
                UnactiveToolAndToolBar();
            }
            else if (cropState as CropThirstyState)
            {
                if (toolItemBtn && toolItemBtn.GetToolType() == ToolItemUI.ToolType.WATERING)
                {
                    //transform.LookAt(currentField.transform.position);
                    SetPlayerLookView();
                    m_animator.SetTrigger(wateringParam);
                } else
                {
                    ToastManager.Instance.ShowNotify("INVALID TOOL", currentField.transform.position + Vector3.up);
                }
            }
            else if (cropState as CropDiseasedState)
            {
                if (toolItemBtn && toolItemBtn.GetToolType() == ToolItemUI.ToolType.MEDICINE) // tam thoi de watering nhu nay
                {
                    //transform.LookAt(currentField.transform.position);
                    SetPlayerLookView();
                    m_animator.SetTrigger(healingParam);
                } else
                {
                    ToastManager.Instance.ShowNotify("INVALID TOOL", currentField.transform.position + Vector3.up);
                }
            }
        }
    }
    public bool CheckAndDisplayToolBar()
    {
        if (!toolBarManager.IsShow())
        {
            toolBarManager.Show();
            if (currentToolItemGameObj)
            {
                currentToolItemGameObj.gameObject.SetActive(true);
            }
            StartCoroutine(DelayActivateToolBorder());
            //Debug.LogError("Tool bar is hidingggggggg");
            return false;
        } else
        {
            //Debug.LogError("Tool bar is showinggggggg");
        }
        return true;
    }

    public IEnumerator UnactiveToolAndToolBar()
    {
        yield return new WaitUntil(() => !IsWorking());
        currentToolItemGameObj.SetActive(false);
        toolBarManager.Hide();
    }

    // Event callback invoked from animation
    public void OnHoeingDone()
    {
        Inventory.Instance.AddItem(hoeUse);
        currentField.Hoeing();
        StartCoroutine(UnactiveToolAndToolBar());
    }
    public void OnWateringDone()
    {
        Inventory.Instance.AddItem(waterUse);
        currentField.Watering();
        StartCoroutine(UnactiveToolAndToolBar());
    }
    public void OnHealingDone()
    {
        Inventory.Instance.AddItem(medicineUse);
        Debug.LogError("Healing doneeeeeeeeeeeeeeeeeeeeeeeeeeeee");
        currentField.Healing();
        StartCoroutine(UnactiveToolAndToolBar());
    }
    public void OnSeedingDone()
    {
        playerPlantingComp.PlantCrop(currentField, seedItem.GetCropType());
        Inventory.Instance.SubtractQuantity(seedItem, 1);
        seedBarManager.UpdateSeedQuantity();
        seedBarManager.Hide();
        if (seedItemBtn.GetQuantity() <= 0)
        {
            seedBarManager.SetChoosedItem(seedBarManager.GetFirstSeedItemBtn());
            StartCoroutine(DelayActivateSeedBorder());
        }
    }


    private void ResetAllTriggers()
    {
        foreach (var trigger in m_animator.parameters)
        {
            m_animator.ResetTrigger(trigger.name);
        }
    }

    public void SetAnimSpeed(float speed)
    {
        m_animator.SetFloat(m_animSpeedParam, speed);
    }

    public bool IsWorking()
    {
        return m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Work");
    }

    public bool IsIdle()
    {
        return m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle");
    }


    public void EquipItem(ToolItemUI item)
    {
        toolItemBtn = item;//.GetToolType();
        if (currentToolItemGameObj != null)
        {
            currentToolItemGameObj.gameObject.SetActive(false);
        }
        switch (item.GetToolType())
        {
            case ToolItemUI.ToolType.HOE:
                currentToolItemGameObj = hoe;
                break;
            case ToolItemUI.ToolType.WATERING:
                currentToolItemGameObj = wateringCan;
                break;
            case ToolItemUI.ToolType.MEDICINE:
                currentToolItemGameObj = medicineCan;
                break;
            default:
                break;
        }
        if (currentToolItemGameObj)
        {
            currentToolItemGameObj.SetActive(true);
        }
    }
    public void SetSeed(SeedItemBtn seedItem)
    {
        if (currentToolItemGameObj)
        {
            currentToolItemGameObj.SetActive(false);
        }
        seedItemBtn = seedItem;
    }
    public void ResetToIdleAnim()
    {
        //m_animator.SetFloat(m_animSpeedParam, 0f);
        m_animator.Rebind();
        m_animator.Update(0f);
    }
    public void PlayFeedingAnim(Transform target)
    {
        transform.LookAt(target.position);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        //m_animator.SetTrigger(seedParam);
    }
}
