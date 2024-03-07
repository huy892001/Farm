using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chicken : MonoBehaviour
{
    [SerializeField] ChickenItem chickenItem;
    [SerializeField] Animator animator;
    [SerializeField] float minGrowthDuration, maxGrowthDuration, minLayingEggDuration, maxLayingEggDuration;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float moveDistance = 4f;
    [SerializeField] float minSpeed = 1f, maxSpeed = 2f;
    [SerializeField] float eatDuration, layingDuration;
    [SerializeField] float initScale = .2f;
    [SerializeField] Transform headTrans;
    [SerializeField] GameObject hungryIcon;
    [SerializeField] RectTransform canvas;
    [SerializeField] GameObject selectedIcon; 

    float growthDuration = 10f, layingEggDuration = 10f;
    ChickenController chickenController;
    Collider fenceCollider;
    ChickenFood food = null;
    Vector3 targetPosition;
    float curSpeed = 1f;
    Coroutine walkCO;
    bool isFullGrown = false;
    float processTimer = 0f;
    State curState = State.NORMAL;
    float hungryTimer = 0f;
    float startIconScale;

    public enum State
    {
        NORMAL,
        HUNGRY
    }
    public void SetFenceCollider(BoxCollider collider)
    {
        fenceCollider = collider;
    }

    private void Start()
    {
        isFullGrown = false;
        layingEggDuration = Random.Range(minLayingEggDuration, maxLayingEggDuration);
        growthDuration = Random.Range(minGrowthDuration, maxGrowthDuration);
        targetPosition = FindRandomTargetPosition();
        // Debug.Log("laying : " + layingEggDuration);
        // Debug.Log("Grow: " + growthDuration);
        walkCO = StartCoroutine(WalkRandom());
        transform.localScale = Vector3.one * initScale;
        curState = State.HUNGRY;
        hungryIcon.SetActive(true);
        selectedIcon.SetActive(false);
        // startIconScale = 1 / initScale;
        hungryIcon.transform.localScale = Vector3.one;// * startIconScale; 
    }

    public void UpdateSelf()
    { 
        if (curState == State.NORMAL)
        {
            processTimer += Time.deltaTime;
            // Debug.Log("Normal: " + processTimer);
            if (isFullGrown)
            {
                // Debug.Log("1");
                if (processTimer >= layingEggDuration)
                {
                    GoToChickenNestAndLayingEgg();
                    processTimer = 0f;
                }
            } else
            {
                // Debug.Log("2");
                Grown();
                if (processTimer >= growthDuration)
                {
                    hungryIcon.SetActive(true);
                    curState = State.HUNGRY;
                    processTimer = 0f;
                    isFullGrown = true;
                }
            }
        } else
        {
            hungryTimer += Time.deltaTime;
            if (hungryTimer >= .5f)
            {
                hungryTimer = 0f;
                FindFoodToEat();
            }
        }
        if (canvas.rotation != Quaternion.identity)
        {
            canvas.rotation = Quaternion.identity;
        }
    }

    void Grown()
    {
        float scale = Mathf.Lerp(initScale, 1, processTimer / (float)growthDuration);
        // Debug.Log("Scale: " + scale + " " + " growth: " + growthDuration);
        transform.localScale = Vector3.one * scale;
        hungryIcon.transform.localScale = Vector3.one * Mathf.Lerp(startIconScale, 1f, processTimer / growthDuration); 
    }
    
    public void GoToChickenNestAndLayingEgg()
    {
        StopCoroutine(walkCO);
        transform.DOKill();
        GoToChickenNest();
    }

    void GoToChickenNest()
    {
        ChickenNest chickenNest = FindObjectOfType<ChickenNest>();
        Vector3 targetPosition = chickenNest.GetRandomPoint();
        targetPosition.y = transform.position.y;
        Debug.Log("Go to chicken nestttttttttttt: " + targetPosition);
        transform.LookAt(targetPosition);
        animator.SetFloat("speed", 2.0f);
        transform.DOMove(targetPosition, 2.0f).SetSpeedBased().SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Debug.LogError("Move to nest doneeeeeeee");
                StartCoroutine(LayingEgg(chickenNest));
            });
    }
    IEnumerator LayingEgg(ChickenNest chickenNest)
    {
        animator.SetFloat("speed", 0f);
        Debug.Log("Before Laying eggggggggggggggggggggggg");
        yield return new WaitForSeconds(layingDuration);
        Debug.Log("Laying eggggggggggggggggggggggg");
        chickenNest.SpawnEgg(transform.position);
        curState = State.HUNGRY;
        hungryIcon.SetActive(true);
        walkCO = StartCoroutine(WalkRandom(true));
    }
    public IEnumerator WalkRandom(bool immediately = false)
    {
        animator.SetFloat("speed", 0f);
        if (immediately)
        {
            yield return null;
        } else
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
        curSpeed = Random.Range(minSpeed, maxSpeed);
        animator.SetFloat("speed", curSpeed);
        transform.LookAt(targetPosition);
        transform.DOMove(targetPosition, curSpeed).SetSpeedBased().SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                targetPosition = FindRandomTargetPosition();
                walkCO = StartCoroutine(WalkRandom());
            });
    }
    int cnt = 0;

    public Vector3 FindRandomTargetPosition()
    {
        cnt = 0;
        Vector3 nextPoint;
        while (true)
        {
            //Debug.LogError("Inside");
            nextPoint = transform.position + new Vector3(
                Random.Range(-moveDistance, moveDistance),
                0,
                Random.Range(-moveDistance, moveDistance));
            if (fenceCollider.bounds.Contains(nextPoint))
            {
                break;
            }
            cnt++;
            if (cnt > 100)
            {
                Debug.Log("Inside infinite loop");
                break;
            }
        }
        return nextPoint;
    }
    public void FindFoodToEat()
    {
        if (food != null || curState == State.NORMAL)
        {
            return;
        }
        ChickenFood[] foods = FindObjectsOfType<ChickenFood>();
        float minDistance = float.MaxValue;
        for (int i = 0; i < foods.Length; i++)
        {
            if (foods[i].GetState() == ChickenFood.State.NORMAL)
            {
                float curDistance = Vector3.Distance(transform.position, foods[i].transform.position);
                if (curDistance < minDistance)
                {
                    food = foods[i];
                    minDistance = curDistance; 
                }
            }
        }
        if (food != null)
        {
            StopCoroutine(walkCO);
            transform.DOKill();
            food.SetState(ChickenFood.State.OCCUPIED);
            transform.LookAt(food.transform);
            animator.SetFloat("speed", 2.0f);
            Vector3 toFoodVector = (food.transform.position - transform.position).normalized;
            float centerToHeadDistance = Distance(transform.position.x, transform.position.z,
                headTrans.position.x, headTrans.position.z);
            Debug.LogError("Center to head distance: " + centerToHeadDistance);
            transform.DOMove(food.transform.position, 2.0f)
                .SetSpeedBased().SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    StartCoroutine(EatFood());
                });
        }
    }

    float Distance(float x1, float y1, float x2, float y2)
    {
        float ans = Mathf.Sqrt(Mathf.Pow(x1 - x2, 2f) + Mathf.Pow(y1 - y2, 2f));
        return ans;
    }

    public IEnumerator EatFood()
    {
        animator.SetFloat("speed", 0f);
        yield return null;
        animator.SetBool("eat", true);
        yield return new WaitForSeconds(eatDuration);
        curState = State.NORMAL;
        hungryIcon.SetActive(false);
        Destroy(food.gameObject);
        food = null;
        animator.SetBool("eat", false);
        yield return new WaitForSeconds(1f);
        walkCO = StartCoroutine(WalkRandom());
    }
    public ChickenItem GetChickenItem()
    {
        return chickenItem;
    }
    public int GetSellPrice()
    {
        if (isFullGrown)
        {
            return chickenItem.GetSellPrice();
        } else
        {
            return chickenItem.GetBuyPrice() / 2;
        }
    }

    public void SetShowChoosedIconState(bool state)
    {
        selectedIcon.SetActive(state);
    }

    public void SetChickenController(ChickenController controller)
    {
        chickenController = controller;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            chickenController.AddToNearChicken(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            chickenController.RemoveNearChicken(this);
        }
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, moveDistance);
    }
#endif
}
