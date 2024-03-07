using UnityEngine;
using System.Collections.Generic;
using System;

public enum DialogType
{
    SETTING,
    WIN,
    LOSE,
    SHOP,
    INVENTORY,
    SELLING,
    COOKING,
    STARTGAME
};

public enum DialogShow
{
    DONT_SHOW_IF_OTHERS_SHOWING,
    REPLACE_CURRENT,
    STACK,
    SHOW_PREVIOUS,
    OVER_CURRENT
};

public class DialogController : PersistentSingleton<DialogController> 
{
    // public static DialogController Instance;

    [HideInInspector]
    public Dialog currentDialog;
    [HideInInspector]
    public Dialog[] baseDialogs;

    public Action onDialogsOpened;
    public Action onDialogsClosed;
    public Stack<Dialog> dialogs = new Stack<Dialog>();

    public void ShowDialog(int type)
    {
        ShowDialog((DialogType)type, DialogShow.DONT_SHOW_IF_OTHERS_SHOWING);
    }

    public Dialog ShowDialog(DialogType type, DialogShow option = DialogShow.REPLACE_CURRENT)
    {
        //if (ConfigController.instance.IsShowingTutorialLevelOne == true) return;

        Dialog dialog = GetDialog(type);
        ShowDialog(dialog, option);
        return dialog;
        //if (Sound.instance != null)
        //{
        //    Sound.instance.Play(Sound.instance.ui_slider_show);
        //}
        //if (TextPreview.instance)
        //{
        //    TextPreview.instance.SetText("");
        //}
    }
    public void ShowDialog(Dialog dialog, DialogShow option = DialogShow.REPLACE_CURRENT)
    {
        if (currentDialog != null)
        {
            if (option == DialogShow.DONT_SHOW_IF_OTHERS_SHOWING)
            {
                Destroy(dialog.gameObject);
                return;
            }
            else if (option == DialogShow.REPLACE_CURRENT)
            {
                currentDialog.Close();
            }
            else if (option == DialogShow.STACK)
            {
                currentDialog.Hide();
            }
        }

        currentDialog = dialog;
        if (option != DialogShow.SHOW_PREVIOUS)
        {
            currentDialog.onDialogOpened += OnOneDialogOpened;
            currentDialog.onDialogClosed += OnOneDialogClosed;
            dialogs.Push(currentDialog);
        }

        if (currentDialog == null)
        {
            Debug.Log("ERROR ShowDialog");
        }
        else
        {
            currentDialog.Show();
        }

        if (onDialogsOpened != null)
            onDialogsOpened();
    }

    public Dialog GetDialog(DialogType type)
    {
        Dialog dialog = baseDialogs[(int)type];
        dialog.dialogType = type;
        return (Dialog)Instantiate(dialog, transform.position, transform.rotation);
    }

    public void CloseCurrentDialog()
    {
        if (currentDialog != null)
            currentDialog.Close();
    }

    public void CloseDialog(DialogType type)
    {
        if (currentDialog == null) return;
        if (currentDialog.dialogType == type)
        {
            currentDialog.Close();
        }
    }

    public bool IsDialogShowing()
    {
        return currentDialog != null;
    }

    public bool IsDialogShowing(DialogType type)
    {
        if (currentDialog == null) return false;
        return currentDialog.dialogType == type;
    }

    private void OnOneDialogOpened(Dialog dialog)
    {
        //Debug.Log ("OnOneDialogOpened");
    }

    private void OnOneDialogClosed(Dialog dialog)
    {
        if (currentDialog == dialog)
        {
            currentDialog = null;
            dialogs.Pop();
            if ((onDialogsClosed != null && dialogs.Count == 0))
            {
                onDialogsClosed();
            }

            //if (dialogs.Count > 0)
            //{
            //    ShowDialog(dialogs.Peek(), DialogShow.SHOW_PREVIOUS);
            //}
        }
    }

    public void CleanUpDialog()
    {
        if (dialogs.Count > 0)
        {
            dialogs.Clear();
        }
    }
}

