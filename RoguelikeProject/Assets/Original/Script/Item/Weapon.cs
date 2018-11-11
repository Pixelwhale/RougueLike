using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    private int id;
    private int power;
    private int range;

    public Weapon(int id, int power, int range)
    {
        this.id = id;
        this.power = power;
        this.range = range;
    }

    public int ID() { return id; }
    public int Power() { return power; }
    public int Range() { return range; }
}
