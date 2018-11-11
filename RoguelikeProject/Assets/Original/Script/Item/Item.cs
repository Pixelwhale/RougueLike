using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    protected int ID;
    protected int quantity;

    public Item(int ID, int quantity)
    {
        this.ID = ID;
        this.quantity = quantity;
    }

    public int GetID() { return ID; }
    public int GetQuantity() { return quantity; }

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