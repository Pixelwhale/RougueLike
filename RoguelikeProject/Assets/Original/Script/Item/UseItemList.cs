using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseItemList : MonoBehaviour
{

    [SerializeField]
    private GameObject[] itemPoint;

    [SerializeField]
    private Sprite defaultSprite;

    [SerializeField]
    private ItemTrash trash;

    [SerializeField]
    private AudioClip eat1, eat2, drink1, drink2;

    //ButtonのGameObjectが持っているImage
    private Image[] itemUIs;

    //ButtonのGameObjectが持っているItemSystem
    private ItemSystem[] systems;

    private ItemUse[] itemList;

    private ItemData data;

    private GameObject player;

    private DragSprite dragSprite;

    void Start()
    {
        data = GameObject.Find("GameDataManager").GetComponent<ItemData>();
        player = GameObject.FindGameObjectWithTag("Player");
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
    }

    private void SetItemList()
    {
        itemList = Completed.GameManager.instance.UseItemList;
    }

    private void SetAction()
    {
        foreach (var system in systems)
        {
            system.click = () => Click(system.index);
            system.dragBegin = () => DragBegin(system.index);
            system.drag = Drag;
            system.dragEnd = () => DragEnd(system.index);
        }
    }

    void Update()
    {
        SpriteUpdate();
        ItemNumTextUpdate();
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

    private void SetSprite(Image image, Sprite sprite)
    {
        image.sprite = sprite;
    }

    //アイテムの個数を表記するテキストを更新
    private void ItemNumTextUpdate()
    {
        for(int i = 0; i < itemList.Length; i++)
        {
            string write = itemList[i].num.ToString();
            systems[i].WriteText(write);
        }
    }

    private void Click(int index)
    {
        //indexが範囲内でなかったらreturn
        if (!utility.Range.IsRangeOfInt(index, 0, itemList.Length - 1)) return;

        ItemType type = itemList[index].type;
        if (type == ItemType.NONE) return;

        //アイテムを使用する
        Completed.GameManager.instance.UseItem(type);

        if (type == ItemType.FOOD1 || type == ItemType.FOOD2)
        {
            RemakePlayer remakeplayer = player.GetComponent<RemakePlayer>();
            remakeplayer.EatFood(data.ItemInfoData[type].value);

            //フードを食べた音を再生
            Completed.SoundManager.instance.RandomizeSfx(eat1, eat2);
        }
        if (type == ItemType.PORTION)
        {
            RemakePlayer remakeplayer = player.GetComponent<RemakePlayer>();
            remakeplayer.RecoveryHP(data.ItemInfoData[type].value);

            //飲み物再生
            Completed.SoundManager.instance.RandomizeSfx(drink1, drink2);
        }
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
            Trash(index);
        }

        //spriteをデフォルトに設定
        dragSprite.ResetSprite();
        dragSprite.dragItemType = ItemType.NONE;
    }

    private void Trash(int index)
    {
        //indexが範囲内でなかったらreturn
        if (!utility.Range.IsRangeOfInt(index, 0, itemList.Length - 1)) return;

        itemList[index].type = ItemType.NONE;

        itemList[index].num = 0;
    }
}
