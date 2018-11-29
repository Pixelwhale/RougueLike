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

    private Image[] itemUIs;

    private ItemUse[] itemList;

    private ItemData data;

    private GameObject player;

    void Start()
    {
        data = GameObject.Find("GameDataManager").GetComponent<ItemData>();
        player = GameObject.FindGameObjectWithTag("Player");

        itemUIs = new Image[itemPoint.Length];

        for (int i = 0; i < itemPoint.Length; i++)
        {
            itemUIs[i] = itemPoint[i].GetComponent<Image>();
        }

        SetItemList();
    }

    private void SetItemList()
    {
        itemList = Completed.GameManager.instance.UseItemList;
    }

    private void SetClick()
    {
        foreach(var obj in itemPoint)
        {
            ItemSystem system = obj.GetComponent<ItemSystem>();
            system.click = () => Click(system.index);
        }
    }

    // Update is called once per frame
    void Update()
    {
        InfoUpdate();
    }

    private void InfoUpdate()
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

    private void Click(int index)
    {
        //indexが範囲内でなかったらreturn
        if (!utility.Range.IsRangeOfInt(index, 0, itemList.Length - 1)) return;

        ItemType type = itemList[index].type;

        //アイテムを使用する
        Completed.GameManager.instance.UseItem(type);

        if (type == ItemType.FOOD1 || type == ItemType.FOOD2)
        {

        }
        if (type == ItemType.PORTION)
        {

        }
    }
}
