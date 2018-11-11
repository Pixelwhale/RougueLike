using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBackpack
{
    public Completed.Player player;

    private List<Item> items = new List<Item>(20);

    public void Initialize()
    {
        items.Clear();
    }

    public void AddItem(Item item)
    {
        foreach (Item i in items)
        {
            if (i.GetID() == item.GetID())
            {
                i.AddQuantity(item.GetQuantity());
                return;
            }
        }
        AddNewItem(item);
    }
    private void AddNewItem(Item item)
    {
        if (items.Count == items.Capacity)
        {
            Debug.Log("Item list is full");
            return;
        }
        items.Add(item);
    }

    public void UseItem(int index)
    {
        Debug.Assert(index >= 0 && index < items.Count, "UseItem : index invalid");
        items[index].Use(player);
    }
}