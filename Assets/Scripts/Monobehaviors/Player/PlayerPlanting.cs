using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlanting : MonoBehaviour
{
    [SerializeField] LayerMask fieldLayermark;
    [SerializeField, Range(.1f, 2f)] float plantingRange;
    [SerializeField] CropFactory cropFactory;
    [SerializeField] Inventory inventory;

    //[SerializeField] Color fieldHighlightColor, hasRipedCropColor;

    Camera mainCamera;
    Collider[] inPlantingRangeColliders = new Collider[100];
    Collider lastClosestField = null;


    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        int collidersCnt = Physics.OverlapSphereNonAlloc(transform.position, plantingRange, inPlantingRangeColliders, fieldLayermark, QueryTriggerInteraction.Collide);
        if (collidersCnt > 0)
        {
            //Debug.Log("Count: " + collidersCnt);
            Collider curClosestField = GetClosestField(collidersCnt);
            if (lastClosestField != null && lastClosestField != curClosestField)
            {
                //lastClosestField.GetComponent<Renderer>().material.color = Color.white;
                lastClosestField.GetComponent<Field>().SetToNormalColor();
            }
            lastClosestField = curClosestField;
            Field curFieldComp = lastClosestField.GetComponent<Field>();
            if (!curFieldComp.HasDiedCrop)
            {
                lastClosestField.GetComponent<Field>().ChangeToHighlightColor();//GetRenderer().material.color = fieldHighlightColor;
            }
            if (curFieldComp)
            {
                if (curFieldComp.HasCrop && curFieldComp.GetCurrentCrop().IsRiped)
                {
                    lastClosestField.GetComponent<Field>().ChangeToRipedColor();//.GetRenderer().material.color = hasRipedCropColor;
                } 
            }
        } else
        {
            if (lastClosestField != null)
            {
                //lastClosestField.GetComponent<Renderer>().material.color = Color.white;
                lastClosestField.GetComponent<Field>().SetToNormalColor();
                lastClosestField = null;
            }
        }
    }

    public void PlantCrop(Field field, CropType type)
    {
        Crop cropInstance = cropFactory.GetCrop(type);
        Field curFieldComp = field; 
        curFieldComp.SetCrop(cropInstance);
        cropInstance.SetField(curFieldComp);
        cropInstance.transform.SetParent(curFieldComp.transform);
        cropInstance.transform.localPosition = Vector3.up * .15f;
        cropInstance.transform.localScale = Vector3.zero;
    }
    
    public Field GetCurrentInteractableField()
    {
        if (lastClosestField)
        {
            return lastClosestField.GetComponent<Field>();
        }
        return null;
    } 

    private Collider GetClosestField(int collidersCnt)
    {
        float minDistance = float.MaxValue;
        int minIndex = 0;
        for (int i = 0; i < collidersCnt; i++)
        {
            Vector3 currentColliderPos = inPlantingRangeColliders[i].transform.position;
            float curDistance = Vector3.Distance(transform.position, currentColliderPos);
            if (minDistance > curDistance)
            {
                minDistance = curDistance;
                minIndex = i;
            }
        }
        return inPlantingRangeColliders[minIndex];
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, plantingRange);
    }
}
