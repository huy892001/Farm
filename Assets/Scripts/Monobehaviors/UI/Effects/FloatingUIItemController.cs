using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatingUIItemController : PersistentSingleton<FloatingUIItemController> 
{
    [SerializeField] FloatingUIItem floatingItemPrefab;
    [SerializeField] Transform fromPos, toPos;
    [SerializeField] Sprite goldSprite;

    RectTransform rectTrans;
    Camera mainCamera;

    protected override void Awake()
    {
        base.Awake();
        rectTrans = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    //public void Test()
    //{
    //    Show(sprite, -2, fromPos.position, toPos.position, null);
    //}

    public void Show(Sprite sprite, int num, Vector2 fromPosition, Vector2 toPosition, Action onCompleted = null)
    {
        FloatingUIItem instance = Instantiate(floatingItemPrefab, transform);
        instance.SetData(sprite, num);
        RectTransform rectTransform = instance.GetComponent<RectTransform>();
        rectTransform.position = fromPosition;
        rectTransform.DOMove(toPosition, .5f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                instance.GetComponent<CanvasGroup>().DOFade(.0f, .2f)
                .OnComplete(() =>
                {
                    onCompleted?.Invoke();
                    Destroy(instance.gameObject);
                });
            });
    }
    public void ShowWithAnchoredPosition(Sprite sprite, int num, Vector2 fromPosition, Vector2 toPosition, Action onCompleted = null)
    {
        FloatingUIItem instance = Instantiate(floatingItemPrefab, transform);
        instance.SetData(sprite, num);
        RectTransform rectTransform = instance.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = fromPosition;
        rectTransform.DOAnchorPos(toPosition, .5f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                instance.GetComponent<CanvasGroup>().DOFade(.0f, .2f)
                .OnComplete(() =>
                {
                    onCompleted?.Invoke();
                    Destroy(instance.gameObject);
                });
            });
    }
    public void ShowGold(int num, Vector3 initPosition, int moveUpAmount, float duration, Action onCompleted = null)
    {
        ShowAnchored(goldSprite, num, initPosition, moveUpAmount, duration, onCompleted);
    } 
    public void ShowAnchored(Sprite sprite, int num, Vector3 initPosition, int moveUpAmount, float duration, Action onCompleted = null)
    {
        Debug.LogError("Init position: " + initPosition);
        Vector2 canvasPosition = mainCamera.WorldToViewportPoint(initPosition);
        canvasPosition.x = canvasPosition.x * rectTrans.rect.width - rectTrans.rect.width / 2.0f;
        canvasPosition.y = canvasPosition.y * rectTrans.rect.height - rectTrans.rect.height / 2.0f;

        FloatingUIItem instance = Instantiate(floatingItemPrefab, transform);
        instance.SetData(sprite, num);
        RectTransform rectTransform = instance.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = canvasPosition;
        rectTransform.DOAnchorPos(canvasPosition + Vector2.up * moveUpAmount, duration).SetEase(Ease.OutSine)
            .OnComplete(() =>
            {
                instance.GetComponent<CanvasGroup>().DOFade(.0f, .2f)
                .OnComplete(() =>
                {
                    onCompleted?.Invoke();
                    Destroy(instance.gameObject);
                });
            });
    }
}
