using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSystem : MonoBehaviour
{
    public int index;
    public System.Action click;

    public System.Action dragBegin;
    public System.Action drag;
    public System.Action dragEnd;

    private Text text;

	void Awake ()
    {
        click = null;
	}

    private void Start()
    {
        text = GetComponentInChildren<Text>();
    }

    private void InvokeAction(System.Action action)
    {
        if (action != null)
        {
            action.Invoke();
        }
    }

    //クリックされたときの処理
    public void Click()
    {
        InvokeAction(click);
    }

    public void DragBegin()
    {
        InvokeAction(dragBegin);
    }

    public void Drag()
    {
        InvokeAction(drag);
    }

    public void DragEnd()
    {
        InvokeAction(dragEnd);
    }

    public void WriteText(string write)
    {
        text.text = write;
    }
}
