using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EqipmentItemList : MonoBehaviour
{
    [SerializeField]
    private GameObject[] itemPoint;

    [SerializeField]
    private Sprite defaultSprite;

    [SerializeField]
    private ItemTrash trash;

    private Image[] itemUIs;
    private ItemSystem[] systems;

    private ItemEqipment[] itemList;

    private ItemData data;

    private Status status;

    private DragSprite dragSprite;

    void Start()
    {
        data = GameObject.Find("GameDataManager").GetComponent<ItemData>();
        status = GameObject.FindGameObjectWithTag("Player").GetComponent<Status>();
        dragSprite = GameObject.Find("DraggerMouse").GetComponent<DragSprite>();

        itemUIs = new Image[itemPoint.Length];
        systems = new ItemSystem[itemPoint.Length];

        for (int i = 0; i < itemPoint.Length; i++)
        {
            itemUIs[i] = itemPoint[i].GetComponent<Image>();
            systems[i] = itemPoint[i].GetComponent<ItemSystem>();
        }

        SetItemList();
        SetAction();

        InitEqippedCheck();
    }

    private void SetItemList()
    {
        itemList = Completed.GameManager.instance.EqipmentItemList;
    }

    private void SetAction()
    {
        foreach (var system in systems)
        {
            system.click = () => Click(system.index);
            system.dragBegin = ()=> DragBegin(system.index);
            system.drag = Drag;
            system.dragEnd = () => DragEnd(system.index);
        }
    }

    private void InitEqippedCheck()
    {
        foreach (var info in itemList)
        {
            if (!info.isEqipment) continue;

            //WEAPONの場合
            if (IsWeapon(info.type))
            {
                status.Weapon = data.ItemInfoData[info.type].value;
            }

            //ARMORの場合
            if (IsArmor(info.type))
            {
                status.Armor = data.ItemInfoData[info.type].value;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpriteUpdate();

        ItemEqippedUpdate();
    }

    private void SpriteUpdate()
    {
        if (itemList == null)
        {
            SetItemList();
            return;
        }

        for (int i = 0; i < itemList.Length; i++)
        {
            if (itemList[i].type == ItemType.NONE)
            {
                SetSprite(itemUIs[i], defaultSprite);
                continue;
            }

            //NONEでなかった時の処理
            SetSprite(itemUIs[i], data.ItemInfoData[itemList[i].type].sprite);
        }
    }

    //アイテムが装備状態かどうかを更新する
    private void ItemEqippedUpdate()
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            string write = itemList[i].isEqipment ? "E" : "";
            systems[i].WriteText(write);
        }
    }

    private void SetSprite(Image image, Sprite sprite)
    {
        image.sprite = sprite;
    }

    //ItemSystemのクリック関数
    private void Click(int index)
    {
        //indexが範囲内でなかったらreturn
        if (!utility.Range.IsRangeOfInt(index, 0, itemList.Length - 1)) return;

        ItemType type = itemList[index].type;
        if (type == ItemType.NONE) return;

        if (IsWeapon(type))  
        {
            //WEAPON
            bool isEqipped = IsAlreadyEqipped(type, index);
            int weaponValue = isEqipped ? 0 : data.ItemInfoData[type].value;
            WeaponTakeOff();
            status.Weapon = weaponValue;

            if (isEqipped) return;
        }
        if (IsArmor(type))
        {
            //ARMOR
            bool isEqipped = IsAlreadyEqipped(type, index);
            int armorValue = isEqipped ? 0 : data.ItemInfoData[type].value;
            ArmorTakeOff();
            status.Armor = armorValue;

            if (isEqipped) return;
        }

        //装備する
        Eqipped(type, index);
    }

    //武器の装備を外す
    private void WeaponTakeOff()
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            if (IsWeapon(itemList[i].type))
            {
                //装備を外す
                itemList[i].isEqipment = false;
            }
        }
    }

    //防具を外す
    private void ArmorTakeOff()
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            if (IsArmor(itemList[i].type))
            {
                //装備を外す
                itemList[i].isEqipment = false;
            }
        }
    }

    //装備する
    private void Eqipped(ItemType type, int index)
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            if (i != index) continue;
            if (itemList[i].type == type) 
            {
                //装備する
                itemList[i].isEqipment = true;
            }
        }
    }

    //すでに装備されているか
    private bool IsAlreadyEqipped(ItemType type, int index)
    {
        for (int i = 0; i < itemList.Length; i++)
        {
            if (i != index) continue;
            if (itemList[i].type == type)
            {
                if (itemList[i].isEqipment) return true;
            }
        }

        return false;
    }

    bool IsWeapon(ItemType type)
    {
        return utility.Range.IsRangeOfInt((int)type, 0, 8);
    }
    bool IsArmor(ItemType type)
    {
        return utility.Range.IsRangeOfInt((int)type, 9, 13);
    }

    void DragBegin(int index)
    {
        //indexが範囲内でなかったらreturn
        if (!utility.Range.IsRangeOfInt(index, 0, itemList.Length - 1)) return;

        ItemType type = itemList[index].type;
        if (type == ItemType.NONE) return;

        //spriteを設定
        dragSprite.sprite = data.ItemInfoData[type].sprite;
        dragSprite.dragItemType = type;
    }

    void Drag()
    {
        dragSprite.transform.position = Input.mousePosition;
    }

    void DragEnd(int index)
    {
        //indexが範囲内でなかったらreturn
        if (!utility.Range.IsRangeOfInt(index, 0, itemList.Length - 1)) return;

        ItemType type = itemList[index].type;
        if (type == ItemType.NONE) return;

        if (trash.IsOnMouse())
        {
            Trash(dragSprite.dragItemType, index);
        }

        //spriteをデフォルトに設定
        dragSprite.ResetSprite();
        dragSprite.dragItemType = ItemType.NONE;
    }

    private void Trash(ItemType type, int index)
    {
        //indexが範囲内でなかったらreturn
        if (!utility.Range.IsRangeOfInt(index, 0, itemList.Length - 1)) return;

        itemList[index].type = ItemType.NONE;

        //装備中だったら
        if (itemList[index].isEqipment)
        {
            if (IsWeapon(type)) status.Weapon = 0;
            if (IsArmor(type)) status.Armor = 0;
        }

        itemList[index].isEqipment = false;
    }
}
