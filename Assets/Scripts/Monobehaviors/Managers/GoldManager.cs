using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    [SerializeField] float timeToChangeGoldText = 1f;
    [SerializeField] GameEvent onInventoryItemChanged;
    int currentGold;
    GoldDisplayer goldDisplayer;

    private void Awake()
    {
        goldDisplayer = FindObjectOfType<GoldDisplayer>();
    }

    public int CurrentGold
    {
        set
        {
            Debug.LogError("Set current gold: " + value);
            currentGold = value;
            goldDisplayer.SetGold(currentGold);
        }
        get
        {
            return currentGold;
        }
    }

    public void AddGold(int amount)
    {
        Debug.Log("Current gold: " + currentGold);
        StartCoroutine(ChangeGoldText(currentGold, currentGold + amount, timeToChangeGoldText));
        currentGold += amount;
    } 
    public void SubtractGold(int amount)
    {
        StartCoroutine(ChangeGoldText(currentGold, currentGold - amount, timeToChangeGoldText));
        currentGold -= amount;
    } 

    public bool HasEnoughGold(int amount)
    {
        return currentGold >= amount;
    }

    public IEnumerator ChangeGoldText(int startValue, int toValue, float time)
    {
        float timer = 0;
        goldDisplayer.SetGold(startValue);
        while (timer <= 1f)
        {
            timer += Time.deltaTime / time;
            int currentValue = (int)Mathf.Lerp(startValue, toValue, timer);
            goldDisplayer.SetGold(currentValue);
            yield return null;
        }
        onInventoryItemChanged.NotifyAll();
    }
    public float GetAnimLength()
    {
        return timeToChangeGoldText;
    }
}
