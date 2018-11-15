using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBackpack : MonoBehaviour
{
    private List<Weapon> weapons = new List<Weapon>(4);

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        weapons.Clear();
        for (int i = 0; i < weapons.Capacity; ++i)
            weapons.Add(null);
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
        Debug.Assert(index >= 0 && index < weapons.Capacity, "RemoveWeapon : index invalid");

        //空いてる枠をクリックするとき
        if (weapons[index] == null) return;

        //装備していない装備をクリックするとき
        if (IsEquiped(index))
            weapons[index].Equip(false);

        weapons.RemoveAt(index);
    }

    public void Equip(int index)
    {
        Debug.Assert(index >= 0 && index < weapons.Capacity, "Equip : index invalid");

        //空いてる枠をクリックするとき
        if (weapons[index] == null) return;

        //装備している装備をクリックするとき
        if (IsEquiped(index))
        {
            weapons[index].Equip(false);
            return;
        }

        //装備していない装備をクリックするとき
        foreach (Weapon w in weapons)
        {
            w.Equip(false);
        }
        weapons[index].Equip(true);
    }

    public bool IsEquiped(int index)
    {
        return weapons[index].IsEquiped();
    }
}
