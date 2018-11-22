using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    private int id;
    private int power;
    private int range;
    private bool isEquiped = false;

    public Weapon(int id, int power, int range)
    {
        this.id = id;
        this.power = power;
        this.range = range;
    }

    public int ID() { return id; }
    public int Power() { return power; }
    public int Range() { return range; }
    public bool IsEquiped() { return isEquiped; }

    public void Equip(bool b)
    {
        isEquiped = b;
    }
}
