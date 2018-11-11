using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBackpack
{
    private List<Weapon> weapons = new List<Weapon>(4);

    public void Initialize()
    {
        weapons.Clear();
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

    public void RemoveWeapon(int index)
    {
        Debug.Assert(index >= 0 && index < weapons.Count, "RemoveWeapon : index invalid");
        weapons.RemoveAt(index);
    }

    public void Equip(int index)
    {
        Debug.Assert(index >= 0 && index < weapons.Count, "Equip : index invalid");
        foreach (Weapon w in weapons)
        {
            w.Equip(false);
        }
        weapons[index].Equip(true);
    }

    public bool IsEquiped(int index)
    {
        Debug.Assert(index >= 0 && index < weapons.Count, "IsEquiped : index invalid");
        return weapons[index].IsEquiped();
    }
}
