using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    SWORD1,
    SWORD2,
    SWORD3,
    SWORD4,
    SWORD5,
    SWROD6,
    SWORD7,
    SWORD8,
    SWORD9,

    ARMOR1,
    ARMOR2,
    ARMOR3,
    ARMOR4,
    ARMOR5,

    PORTION,

    FOOD1,
    FOOD2,

    SIZE,
    NONE,
}

//インスペクターに登録するアイテムの情報
[System.Serializable]
public struct ItemInfo
{
    public ItemType type;
    public Sprite sprite;
    public int value;
}

//アイテムのタイプと個数を持つ構造体
public struct ItemUse
{
    public ItemType type;
    public int num;

    public ItemUse(ItemType type,int num)
    {
        this.type = type;
        this.num = num;
    }
}

public struct ItemEqipment
{
    public ItemType type;

    //装備中かどうか
    public bool isEqipment;

    public ItemEqipment(ItemType type,bool isEqipment)
    {
        this.type = type;
        this.isEqipment = isEqipment;
    }
}

public class ItemData : MonoBehaviour
{
    [SerializeField]
    private List<ItemInfo> itemInfo;

    private Dictionary<ItemType, ItemInfo> itemData;

    public int itemVarious = (int)ItemType.SIZE;

    public Dictionary<ItemType,ItemInfo> ItemInfoData
    {
        get { return itemData; }
    }

    private void Awake()
    {
        itemData = new Dictionary<ItemType, ItemInfo>();
        RegisterItemData();
    }

    private void RegisterItemData()
    {
        foreach (var info in itemInfo)
        {
            itemData[info.type] = info;
        }
    }
}
