using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public int index;

    private bool WeaponExist()
    {
        Debug.Assert(index >= 0 && index < 4);
        return (GetComponentInParent<WeaponBackpack>().IsExist(index));
    }

    public void Click()
    {
        if (!WeaponExist())
        {
            Debug.Log("Weapon not exist");
            return;
        }
        Debug.Log("click");
        GetComponentInParent<WeaponBackpack>().Equip(index);
    }

    public void BeginDrag()
    {
        if (!WeaponExist())
        {
            Debug.Log("Weapon not exist");
            return;
        }
        Debug.Log("begin");
        Color color = GetComponent<Image>().color;
        color.a = 0.6f;
        GetComponent<Image>().color = color;
    }

    public void EndDrag()
    {
        if (!WeaponExist())
        {
            Debug.Log("Weapon not exist");
            return;
        }
        Debug.Log("end");
        Color color = GetComponent<Image>().color;
        color.a = 1.0f;
        GetComponent<Image>().color = color;
    }

    public void Drop()
    {
        if (!WeaponExist())
        {
            Debug.Log("Weapon not exist");
            return;
        }
        Debug.Log("drop");
        GetComponentInParent<WeaponBackpack>().RemoveWeapon(index);
    }
}
