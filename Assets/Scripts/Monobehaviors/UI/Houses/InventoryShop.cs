using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryShop : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement)
        {
            playerMovement.SetMovingState(false);
            DialogController.Instance.ShowDialog(DialogType.INVENTORY);
        }
    }
}
