using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public int index;

    private bool ItemExist()
    {
        return (GetComponentInParent<ItemBackpack>().IsExist(index));
    }

    public void Click()
    {
        if (!ItemExist())
        {
            Debug.Log("Item not exist");
            return;
        }
		Debug.Log("click");
		GetComponentInParent<ItemBackpack>().UseItem(index);
    }

    public void BeginDrag()
    {
        if (!ItemExist())
        {
            Debug.Log("Item not exist");
            return;
        }
		Debug.Log("begin");
        Color color = GetComponent<Image>().color;
        color.a = 0.6f;
        GetComponent<Image>().color = color;
    }

    public void EndDrag()
    {
        if (!ItemExist())
        {
            Debug.Log("Item not exist");
            return;
        }
		Debug.Log("end");
        Color color = GetComponent<Image>().color;
        color.a = 1.0f;
        GetComponent<Image>().color = color;
    }

    public void Drop()
    {
        if (!ItemExist())
        {
            Debug.Log("Item not exist");
            return;
        }
		Debug.Log("drop");
		GetComponentInParent<ItemBackpack>().RemoveItem(index);
    }
}
