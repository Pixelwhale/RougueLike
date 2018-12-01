using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTrash : MonoBehaviour
{
    private DragSprite dragSprite;
    private RectTransform recttrans;

    private bool current, previous;

    void Start ()
    {
        dragSprite = GameObject.Find("DraggerMouse").GetComponent<DragSprite>();
        recttrans = GetComponent<RectTransform>();
        current = false;
        previous = false;
    }
	
	void Update ()
    {
        previous = current;
        current = IsOnMouse();

        if (IsMouseEnter())
        {
            MouseEnter();
        }

        if (IsMouseExit())
        {
            MouseExit();
        }
	}

    public bool IsOnMouse()
    {
        Vector2 size = recttrans.sizeDelta;
        Vector2 scale = new Vector2(0.5f, 0.5f);
        Vector2 leftTop = (Vector2)recttrans.position - Vector2.Scale(size, scale);
        Rect rect = new Rect(leftTop, size);

        //ItemUIのサイズ
        size = new Vector2(80, 80);
        leftTop = (Vector2)Input.mousePosition - Vector2.Scale(size, scale);
        Rect mouseRect = new Rect(leftTop, size);

        return rect.Overlaps(mouseRect);
    }
    private bool IsMouseEnter()
    {
        return !previous && current;
    }

    private bool IsMouseExit()
    {
        return previous && !current;
    }

    private void MouseEnter()
    {
        dragSprite.IsOnTrash = true;
    }

    private void MouseExit()
    {
        dragSprite.IsOnTrash = false;
    }
}
