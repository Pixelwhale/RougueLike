using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    private int id;
    private Sprite s;
    protected int quantity;

    public Item(int id, Sprite sprite, int quantity)
    {
        this.id = id;
        this.s = sprite;
        this.quantity = quantity;
    }

    public int ID() { return id; }
    public Sprite sprite { get { return s; } private set { s = value; } }
    public int Quantity() { return quantity; }

    public void AddQuantity(int q)
    {
        Debug.Assert(q > 0, "Item.AddQuantity(int q) : q <= 0");
        quantity += q;
    }

    public abstract void Use(Completed.Player player);
    protected void UseQuantity(int q)
    {
        Debug.Assert(q > 0, "Item.UseQuantity(int q) : q <= 0");
        if (quantity < q)
        {
            Debug.Log("Quantity is not enough");
            return;
        }
        quantity -= q;
    }
}