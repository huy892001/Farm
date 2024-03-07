using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FenceGate : MonoBehaviour
{
    [SerializeField] Transform door1, door2; 
    [SerializeField] Transform door1OpenPosition, door2OpenPosition;
    Vector3 initDoor1Pos, initDoor2Pos;
    private void Start() {
        initDoor1Pos = door1.position;
        initDoor2Pos = door2.position;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("fence trigger enter");
            door1.DOMove(door1OpenPosition.position, .25f);
            door2.DOMove(door2OpenPosition.position, .25f);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("fence trigger exit");
            door1.DOMove(initDoor1Pos, .25f);
            door2.DOMove(initDoor2Pos, .25f);
        }
    }
}
