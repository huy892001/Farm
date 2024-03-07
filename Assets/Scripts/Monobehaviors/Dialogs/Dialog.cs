using UnityEngine;
using System;
using TMPro;

public class Dialog : MonoBehaviour
{
    public Animator anim;
    public AnimationClip hidingAnimation;
    public AnimationClip showAnimation;
    public TextMeshProUGUI title, message;
    public Action<Dialog> onDialogOpened;
    public Action<Dialog> onDialogClosed;
    public Action onDialogCompleteClosed;
    public Action onDialogCompleteShowing;
    public Action<Dialog> onButtonCloseClicked;
    public DialogType dialogType;

    private AnimatorStateInfo info;
    private bool _isShowing;

    protected virtual void Awake()
    {
        if (anim == null) anim = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        onDialogCompleteClosed += OnDialogCompleteClosed;
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public virtual void Show()
    {
        if (gameObject == null)
        {
            Debug.Log("loi null");
            return;
        }
        gameObject.SetActive(true);

        if (anim != null && IsIdle())
        {
            _isShowing = true;
            anim.SetTrigger("show");
            onDialogOpened(this);
            if (showAnimation != null)
                Timer.Schedule(this, showAnimation.length, DoOpen);
        }
    }


    public virtual void Close()
    {
        //if (Sound.instance != null)
        //{
        //    Sound.instance.Play(Sound.instance.ui_slider_hide);
        //}
        //Debug.LogError("Is showingggggggg: " + _isShowing);
        if (_isShowing == false)
            return;
        _isShowing = false;
        if (anim != null && IsIdle() && hidingAnimation != null)
        {
            //Debug.LogError("hide with anim");
            anim.SetTrigger("hide");
            Timer.Schedule(this, hidingAnimation.length, DoClose);
        }
        else
        {
            //Debug.LogError("hide with no anim");
            DoClose();
        }

        onDialogClosed(this);
    }


    public void Hide()
    {
        gameObject.SetActive(false);
        _isShowing = false;
    }

    public bool IsIdle()
    {
        info = anim.GetCurrentAnimatorStateInfo(0);
        return info.IsName("Idle");
    }

    public bool IsShowing()
    {
        return _isShowing;
    }

    public virtual void OnDialogCompleteClosed()
    {
        onDialogCompleteClosed -= OnDialogCompleteClosed;
    }

    private void DoClose()
    {
        if (onDialogCompleteClosed != null) onDialogCompleteClosed();
        Destroy(gameObject);
    }

    private void DoOpen()
    {
        if (onDialogCompleteShowing != null) onDialogCompleteShowing();
    }
}

