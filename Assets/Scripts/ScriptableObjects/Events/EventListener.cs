using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    public GameEvent GameEvent;
    public UnityEvent Response;

    private void OnEnable()
    {
        GameEvent.Register(this);
    }

    private void OnDisable()
    {
        GameEvent.Unregister(this);
    }

    public void Notify()
    {
        if (Response != null)
        {
            //Debug.Log("Notify in event listener");
            Response.Invoke();
        }
    }
}
