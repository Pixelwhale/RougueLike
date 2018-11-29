using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    private int id;
    private Sprite s;
    private int power;
    private int range;
    private bool isEquiped = false;

    public Weapon(int id, Sprite sprite, int power, int range)
    {
        this.id = id;
        this.s = sprite;
        this.power = power;
        this.range = range;
    }

    public int ID() { return id; }
    public Sprite sprite { get { return s; } private set { s = value; } }
    public int Power() { return power; }
    public int Range() { return range; }
    public bool IsEquiped() { return isEquiped; }

    public void Equip(bool b)
    {
        isEquiped = b;
    }
}
