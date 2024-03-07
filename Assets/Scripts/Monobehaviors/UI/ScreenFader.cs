using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScreenFader : PersistentSingleton<ScreenFader>
{
    [SerializeField] float duration = .5f;
    [SerializeField] Color fadeColor;
    [SerializeField] Image img_overlay;

    private static GameObject m_canvas;
    //private GameObject m_overlay;
    //Image image;

    protected override void Awake()
    {
        base.Awake();
        //m_canvas = new GameObject("TransitionCanvas");
        //var canvas = m_canvas.AddComponent<Canvas>();
        //canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //DontDestroyOnLoad(m_canvas);
    }

    private void Start()
    {
        img_overlay.gameObject.SetActive(false);
    }

    public IEnumerator FadeIn()
    {
        Debug.LogError("Fade innnnnnnnn");
        img_overlay.gameObject.SetActive(true);

        img_overlay.canvasRenderer.SetAlpha(0.0f);

        var time = 0.0f;
        var halfDuration = duration / 2.0f;
        while (time < halfDuration)
        {
            time += Time.deltaTime;
            img_overlay.canvasRenderer.SetAlpha(Mathf.InverseLerp(0, 1, time / halfDuration));
            yield return new WaitForEndOfFrame();
        }

        img_overlay.canvasRenderer.SetAlpha(1.0f);
    }

    public IEnumerator FadeOut()
    {
        Debug.LogError("Fade out");
        float time = 0.0f;
        while (time < duration / 2f)
        {
            time += Time.deltaTime;
            img_overlay.canvasRenderer.SetAlpha(Mathf.InverseLerp(1, 0, time / duration / 2f));
            yield return new WaitForEndOfFrame();
        }

        img_overlay.canvasRenderer.SetAlpha(0.0f);
        yield return new WaitForEndOfFrame();
        img_overlay.gameObject.SetActive(false);
    }
}