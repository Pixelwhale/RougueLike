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

    private Image[] itemUIs;

    private ItemEqipment[] itemList;

    private ItemData data;

    void Start()
    {
        data = GameObject.Find("GameDataManager").GetComponent<ItemData>();

        itemUIs = new Image[itemPoint.Length];

        for (int i = 0; i < itemPoint.Length; i++)
        {
            itemUIs[i] = itemPoint[i].GetComponent<Image>();
        }

        SetItemList();
    }

    private void SetItemList()
    {
        itemList = Completed.GameManager.instance.EqipmentItemList;
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
}
