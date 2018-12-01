using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSprite : MonoBehaviour
{
    [SerializeField,Range(0.0f,1.0f),Header("ドラッグされたspriteのアルファ値")]
    private float alpha = 0.5f;

    //デフォルトのスプライト
    [SerializeField]
    private Sprite defaultSprite;

    private Image image;

    public ItemType dragItemType;

    //ゴミ箱の上かどうか
    private bool isOnTrash;

    public Sprite sprite
    {
        set { image.sprite = value; }
    }

    public bool IsOnTrash
    {
        set { isOnTrash = value; }
    }

	void Start ()
    {
        image = GetComponent<Image>();
        dragItemType = ItemType.NONE;
        isOnTrash = false;
	}

    private void Update()
    {
        AlphaUpdate();
    }

    public void ResetSprite()
    {
        sprite = defaultSprite;
    }

    void AlphaUpdate()
    {
        float alpha = image.sprite == defaultSprite ? 0.0f : (isOnTrash ? 1.0f : this.alpha);
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
