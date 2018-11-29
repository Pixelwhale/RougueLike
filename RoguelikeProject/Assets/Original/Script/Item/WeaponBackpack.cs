using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBackpack : MonoBehaviour
{
    public List<Weapon> weapons;
    public Sprite space;

    void Awake()
    {
        Initialize();
    }

    private void UpdateSprite()
    {
        Image[] list = GetComponentsInChildren<Image>();
        for (int i = 0; i < weapons.Capacity; ++i)
        {
            if (IsExist(i))
                list[i+2].sprite = weapons[i].sprite;
            else
                list[i+2].sprite = space;
        }
    }

    public void Initialize()
    {
        weapons = new List<Weapon>(4);

        //test
        Sprite sprite = Resources.Load<Sprite>("pikachu");
        AddWeapon(new Weapon(0, sprite, 3, 1));

        UpdateSprite();
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
        UpdateSprite();
    }

    public void RemoveWeapon(int index)
    {
        if (!IsExist(index)) return;

        //空いてる枠をクリックするとき
        if (index >= weapons.Count) return;

        //装備していない装備をクリックするとき
        if (IsEquiped(index))
            weapons[index].Equip(false);

        weapons.RemoveAt(index);
        UpdateSprite();
    }

    public void Equip(int index)
    {
        if (!IsExist(index)) return;

        //空いてる枠をクリックするとき
        if (index >= weapons.Count) return;

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
        if (!IsExist(index))
        {
            Debug.Log("index error");
        }
        return weapons[index].IsEquiped();
    }

    public bool IsExist(int index)
    {
        if (index < 0 || index >= weapons.Capacity) return false;
        return (weapons.Count > index);
    }
}
