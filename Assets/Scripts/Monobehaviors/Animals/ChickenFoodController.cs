using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenFoodController : MonoBehaviour
{
    [SerializeField] GameEvent onChickenFeeded;
    [SerializeField] ChickenFood chickenFoodPrefab;
    [SerializeField] float foodSpawnRange;
    [SerializeField] Transform testPos;
    [SerializeField] Collider fenceBoundaryCollider;
    List<ChickenFood> foods = new List<ChickenFood>();

    public void Test()
    {
        SpawnFoods(10, testPos.position);
    }

    public void SpawnFoods(int numFoods, Vector3 centerPosition)
    {
        for (int i = 0; i < numFoods; i++)
        {
            ChickenFood newFood = Instantiate(chickenFoodPrefab, transform);
            Vector3 foodPosition = FindFoodPosition(centerPosition);
            newFood.transform.position = foodPosition;
            foods.Add(newFood);
        }
        onChickenFeeded.NotifyAll();
    }

    private Vector3 FindFoodPosition(Vector3 centerPosition)
    {
        Vector3 resPosition = centerPosition + 
            new Vector3(
                Random.Range(-foodSpawnRange, foodSpawnRange),
                .1f, 
                Random.Range(-foodSpawnRange, foodSpawnRange));
        while (!fenceBoundaryCollider.bounds.Contains(resPosition))
        {
            resPosition = centerPosition + 
            new Vector3(
                Random.Range(-foodSpawnRange, foodSpawnRange),
                0, 
                Random.Range(-foodSpawnRange, foodSpawnRange));

        }
        return resPosition;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, foodSpawnRange);
    }
#endif
}
