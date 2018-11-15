using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public void BeginDrag()
    {
        Debug.Log("begin");
        Color color = GetComponent<Image>().color;
        color.a = 0.6f;
        GetComponent<Image>().color = color;
    }

    public void EndDrag()
    {
        Debug.Log("end");
        Color color = GetComponent<Image>().color;
        color.a = 1.0f;
        GetComponent<Image>().color = color;
    }
}
