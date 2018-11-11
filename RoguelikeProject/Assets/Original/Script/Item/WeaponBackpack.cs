using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBackpack
{
    private List<Weapon> weapons = new List<Weapon>(4);
    
    //装備している武器のインデックス
    private int equip = -1;

    public void Initialize()
    {
        weapons.Clear();
        equip = -1;
    }

    public void AddWeapon(Weapon weapon)
    {
        //同じ武器持ってるかをチェックする
        foreach (Weapon w in weapons)
        {
            if (w.ID() == weapon.ID())
            {
                Debug.Log("Already have the same weapon in backpack");
                return;
            }
        }

        //武器リストが空いてるかをチェックする
        if (weapons.Count == weapons.Capacity)
        {
            Debug.Log("Weapon backpack is full");
            return;
        }

        //武器の追加
        weapons.Add(weapon);
    }

    public void Equip(int index)
    {
        Debug.Assert(index >= 0 && index < weapons.Count, "Equip : index invalid");
        equip = index;
    }
}
