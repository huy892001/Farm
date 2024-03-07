using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GameEvent : ScriptableObject 
{
    public List<EventListener> eventListeners;

    public void Register(EventListener listener)
    {
        if (!eventListeners.Contains(listener)) eventListeners.Add(listener);
    }

    public void Unregister(EventListener listener)
    {
        if (eventListeners.Contains(listener)) eventListeners.Remove(listener);
    }

    public void NotifyAll()
    {
        foreach (var listener in eventListeners)
        {
            listener.Notify();
        }
    }
}
