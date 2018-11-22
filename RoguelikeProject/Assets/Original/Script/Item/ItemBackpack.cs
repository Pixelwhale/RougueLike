using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBackpack : MonoBehaviour
{
    public Completed.Player player;
    private List<Item> items;
    public Sprite space;

    void Awake()
    {
        Initialize();
    }

    private void UpdateSprite()
    {
        Image[] list = GetComponentsInChildren<Image>();
        for (int i = 0; i < items.Capacity; ++i)
        {
            if (IsExist(i))
                list[i + 2].sprite = items[i].sprite;
            else
                list[i + 2].sprite = space;
        }
    }

    public void Initialize()
    {
        items = new List<Item>(20);
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
        UpdateSprite();
    }

    public void RemoveItem(int index)
    {
        Debug.Assert(index >= 0 && index < items.Count, "RemoveItem : index invalid");
        items.RemoveAt(index);
        UpdateSprite();
    }

    public void UseItem(int index)
    {
        Debug.Assert(index >= 0 && index < items.Count, "UseItem : index invalid");
        items[index].Use(player);
        if (items[index].Quantity() == 0) RemoveItem(index);
    }

    public bool IsExist(int index)
    {
        if (index < 0 || index >= items.Capacity) return false;
        return (items.Count > index);
    }
}