using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("最大HP"),SerializeField]
    private int maxHP = 10;

    [Header("攻撃力"),SerializeField]
    private int attack = 1;

    [Header("防御力"),SerializeField]
    private int defense = 1;

    //現在のHP
    private int currentHP;

    //装備中の攻撃武器
    private int weapon;

    //装備中の防具
    private int armor;

    //現在のHP
    public int CurrentHp
    {
        get { return Mathf.Max(currentHP, 0); }
        set { currentHP = Mathf.Max(value, 0); }
    }

    //攻撃力
    public int Attack
    {
        get { return Mathf.Max(attack + weapon, 0); }
    }

    //守備力
    public int Defense
    {
        get { return Mathf.Max(defense + armor, 0); }
    }

    //武器
    public int Weapon
    {
        set { weapon = value; }
    }

    //防具
    public int Armor
    {
        set { armor = value; }
    }

    void Awake()
    {
        currentHP = maxHP;
        weapon = 0;
        armor = 0;
    }
}
