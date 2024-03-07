using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities 
{
    public static void ValueByTime(float fromValue, float toValue, float duration, Action<float> callback)
    {
        float timer = .0f;
        while (timer <= 1.0f)
        {
            timer += Time.deltaTime / duration;
            float curValue = Mathf.Lerp(fromValue, toValue, timer);
            callback(curValue);
        }
    }
    public static Vector3 GetRandomPointInsideCollider(Collider collider)
    {
        Vector3 extends = collider.bounds.extents;
        Vector3 center = collider.bounds.center;
        Vector3 result = new Vector3(
            UnityEngine.Random.Range(center.x - extends.x, center.x + extends.x), 
            0,
            UnityEngine.Random.Range(center.z - extends.z, center.z + extends.z));
        return result;
    }
    public static void SetYAnchoredPosition(this RectTransform rect, int y)
    {
        Vector2 pos = rect.anchoredPosition;
        pos.y = y;
        rect.anchoredPosition = pos;
    }
    public static Vector3 GetRandomPointInsideCollider(Collider2D collider)
    {
        //Vector3 extents = collider.bounds.extents;
        //Vector3 point = new Vector3(
        //    UnityEngine.Random.Range(-extents.x, extents.x),
        //    //transform.position.y,
        //    UnityEngine.Random.Range(-extents.z, extents.z),
        //    0
        //) + new Vector3(collider.bounds.center.x, 0, collider.bounds.center.z);
        //return collider.transform.TransformPoint(point);
        Vector3 extends = collider.bounds.extents;
        Vector3 center = collider.bounds.center;
        Vector3 result = new Vector3(
            UnityEngine.Random.Range(center.x - extends.x, center.x + extends.x), 
            0,
            UnityEngine.Random.Range(center.y - extends.y, center.y + extends.y));
        return result;
    }
}
