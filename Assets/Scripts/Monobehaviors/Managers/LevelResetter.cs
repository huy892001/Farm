using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelResetter : MonoBehaviour
{
    [SerializeField] GameEvent onNewLevelLoaded;

    private void Start()
    {
        onNewLevelLoaded.NotifyAll();
    }
}
