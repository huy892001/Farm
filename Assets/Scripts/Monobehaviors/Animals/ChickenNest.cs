using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenNest : MonoBehaviour
{
    [SerializeField] ChickenEgg eggPrefab;
    [SerializeField] BoxCollider nestBoundary;
    [SerializeField] BoxCollider modelBoundary;
    [SerializeField] Transform eggParent; 

    List<ChickenEgg> chickenEggs = new List<ChickenEgg>();
    bool isCollectingEggs = false;

    public void Test()
    {
        SpawnEgg(GetRandomPoint());
    }
    public void SpawnEgg(Vector3 pos)
    {
        ChickenEgg newEgg = Instantiate(eggPrefab, eggParent);
        // Debug.Log("Egg position: " + pos.y);
        // newEgg.transform.position = pos;//GetRandomPointInsideCollider(nestBoundary);
        // newEgg.transform.position = Vector3.up * 5;
        pos.y = .18f;
        newEgg.transform.position = pos;
        Debug.Log("Egg position after: " + newEgg.transform.position.y);
        chickenEggs.Add(newEgg);
    }
    public Vector3 GetRandomPoint()
    {
        return Utilities.GetRandomPointInsideCollider(nestBoundary);
    }
    public Collider GetModelBoudary()
    {
        return modelBoundary;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (chickenEggs.Count > 0)
            {
                if (isCollectingEggs) return;
                isCollectingEggs = true;
                ChickenEggItem chickenEggItem = chickenEggs[0].GetEggItem();
                int numEggs = chickenEggs.Count; 
                eggParent.gameObject.SetActive(false);
                if (FloatingUIItemController.Instance)
                {
                    FloatingUIItemController.Instance.ShowAnchored(
                        chickenEggItem.Avatar,
                        numEggs,
                        transform.position, 
                        150
                        , 1.0f, () =>
                        {
                            AddEggsToInventoryAndClear();
                        }); ;
                }
            }
        }
    }
    void AddEggsToInventoryAndClear()
    {
        eggParent.gameObject.SetActive(true);
        isCollectingEggs = false;
        Inventory.Instance.AddItem(chickenEggs[0].GetEggItem(), chickenEggs.Count);
        for (int i = 0; i < chickenEggs.Count; i++)
        {
            Destroy(chickenEggs[i].gameObject);
        }
        chickenEggs.Clear();
    }
}
