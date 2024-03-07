using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenController : MonoBehaviour
{
    [SerializeField] Chicken chickenPrefab;
    [SerializeField] BoxCollider fenceBoundary;
    [SerializeField] BoxCollider chickenNestCollider;
    [SerializeField] GameObject chickenButtonParent;
    [SerializeField] ChickenFoodController chickenFoodController;
    [SerializeField] CropItem riceItem;
    [SerializeField] int numFoodsEachRice = 10;

    List<Chicken> chickens = new List<Chicken>();
    List<Chicken> nearPlayerChickens = new List<Chicken>();

    Chicken selectedChicken = null;
    PlayerController playerController;

    private void Awake()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>(); 
    }

    private void Start()
    {
        chickenButtonParent.SetActive(false);
    }

    private void Update()
    {
        for (int i = 0; i < chickens.Count; i++)
        {
            chickens[i].UpdateSelf();
        }
    }
    public void SpawnChicken()
    {
        int cnt = 0;
        Vector3 chickenPosition = Vector3.zero;
        do
        {
            chickenPosition = GetRandomPointInsideCollider(fenceBoundary);
            Debug.Log("Chicken position: " + chickenPosition);
            cnt++;
            Debug.Log("Cnt: " + cnt);
            if (cnt > 100) {
                Debug.LogError("Inside infinate looppppppppppppppppppppppppp");
                break;
            }
        } while (chickenNestCollider.bounds.Contains(chickenPosition));
        // chickenPosition.y += .1f;
        Chicken newChicken = Instantiate(chickenPrefab, transform);
        newChicken.transform.rotation = Quaternion.identity;
        newChicken.transform.position = chickenPosition;//GetRandomPointInsideCollider(fenceBoundary);
        newChicken.transform.localPosition = 
            new Vector3(newChicken.transform.localPosition.x, 0, newChicken.transform.localPosition.z);
        newChicken.SetFenceCollider(fenceBoundary);
        newChicken.SetChickenController(this);
        chickens.Add(newChicken);
    }
    public Vector3 GetRandomPointInsideCollider(BoxCollider boxCollider)
    {
        Vector3 extents = boxCollider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            //transform.position.y,
            .5f,
            Random.Range(-extents.z, extents.z)
        ) + new Vector3(boxCollider.center.x, 0, boxCollider.center.z);
        Debug.Log("Before: " + point);
        Vector3 worldPoint = boxCollider.transform.TransformPoint(point);
        Debug.Log("After: " + worldPoint);
        return worldPoint; 
    }

    public void AddToNearChicken(Chicken chicken)
    {
        if (selectedChicken == null)
        {
            selectedChicken = chicken;
            selectedChicken.SetShowChoosedIconState(true);
            chickenButtonParent.SetActive(true);
        }
        if (!nearPlayerChickens.Contains(chicken))
        {
            nearPlayerChickens.Add(chicken);
        }
    }

    public void RemoveNearChicken(Chicken chicken)
    { 
        if (nearPlayerChickens.Contains(chicken))
        {
            nearPlayerChickens.Remove(chicken);
            if (chicken == selectedChicken)
            {
                if (nearPlayerChickens.Count > 0)
                {
                    selectedChicken.SetShowChoosedIconState(false);
                    selectedChicken = GetNextRandomChicken(chicken);
                } else
                {
                    selectedChicken.SetShowChoosedIconState(false);
                    selectedChicken = null;
                }
            }
        }
        if (selectedChicken != null)
        {
            selectedChicken.SetShowChoosedIconState(true);
        } else
        {
            chickenButtonParent.SetActive(false);
        }
    }

    public void FindNextChicken()
    {
        if (selectedChicken != null)
        {
            selectedChicken.SetShowChoosedIconState(false);
        }
        selectedChicken = GetNextRandomChicken(selectedChicken);
        if (selectedChicken == null)
        {
            chickenButtonParent.SetActive(false);
        } else
        {
            selectedChicken.SetShowChoosedIconState(true);
        }
    }

    public Chicken GetNextRandomChicken(Chicken chicken)
    {
        if (chicken == selectedChicken && nearPlayerChickens.Count == 1)
        {
            if (nearPlayerChickens.Contains(chicken)) return chicken;
            else return nearPlayerChickens[0];
        } 
        Chicken res = null;
        if (nearPlayerChickens.Count > 1)
        {
            do
            {
                res = nearPlayerChickens[Random.Range(0, nearPlayerChickens.Count)];
            } 
            while (res == chicken);
        }
        return res;
    }

    public void FeedChiken()
    {
        int riceQuantity = Inventory.Instance.GetQuantity(riceItem);
        if (riceQuantity > 0)
        {
            StartCoroutine(COFeedChicken());
        } else
        {
            //Debug.LogError("Not enough riceeeeeeeeeee");
            ToastManager.Instance.ShowNotify("NOT ENOUGH RICE", playerController.transform.position + 2 * Vector3.up, 200, .5f);
        }
    }
    IEnumerator COFeedChicken()
    {
        PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.PlayFeedingAnim(selectedChicken.transform);
        yield return new WaitForSeconds(.3f);
        Inventory.Instance.SubtractQuantity(riceItem, 1);
        chickenFoodController.SpawnFoods(numFoodsEachRice, selectedChicken.transform.position);
    }
    
    // btn sell event
    public void SellChicken()
    {
        if (selectedChicken == null)
        {
            Debug.LogError("Impossibleeeeeeeeeeeeeeeeeeeeeeee");
        } else
        {
            int sellPrice = selectedChicken.GetSellPrice();
            Vector3 chickenPos = selectedChicken.transform.position; 

            ChickenItem chickenItem = selectedChicken.GetChickenItem();
            nearPlayerChickens.Remove(selectedChicken);
            chickens.Remove(selectedChicken);
            Destroy(selectedChicken.gameObject);
            selectedChicken = null;
            FindNextChicken();
            FindObjectOfType<GoldManager>().AddGold(sellPrice);
            FloatingUIItemController.Instance.ShowGold(
                sellPrice,
                chickenPos, 150
                , .5f, () =>
                {
                    Inventory.Instance.AddItem(chickenItem);
                }); ;
        }
    } 
} 