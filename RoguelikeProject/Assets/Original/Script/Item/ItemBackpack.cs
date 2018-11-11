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
        //同じアイテム持ってるかをチェックする
        foreach (Item i in items)
        {
            if (i.ID() == item.ID())
            {
                i.AddQuantity(item.Quantity());
                return;
            }
        }

        //アイテムリストが空いてるかをチェックする
        if (items.Count == items.Capacity)
        {
            Debug.Log("Item backpack is full");
            return;
        }

        //アイテムの追加
        items.Add(item);
    }

    public void RemoveItem(int index)
    {
        Debug.Assert(index >= 0 && index < items.Count, "RemoveItem : index invalid");
        items.RemoveAt(index);
    }

    public void UseItem(int index)
    {
        Debug.Assert(index >= 0 && index < items.Count, "UseItem : index invalid");
        items[index].Use(player);
        if (items[index].Quantity() == 0) RemoveItem(index);
    }
}