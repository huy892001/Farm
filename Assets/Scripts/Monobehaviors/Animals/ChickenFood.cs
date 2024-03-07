using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenFood : MonoBehaviour
{
    [SerializeField] float lifeTime = 10f; 
    float timer;
    public enum State
    {
        NORMAL,
        OCCUPIED
    }

    State state = State.NORMAL;

    private void Start()
    {
        StartCoroutine(CountdownLifeTime());
    }

    public IEnumerator CountdownLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        if (state == State.NORMAL)
        {
            Destroy(gameObject);
        }
    }

    public void SetState(State newState)
    {
        state = newState;
    } 
    public State GetState()
    {
        return state;
    }
}
