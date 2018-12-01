using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemakeItem : MonoBehaviour
{
    [SerializeField]
    private ItemType type;

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!utility.Judgement.IsCompareTag(collision.gameObject, "Player")) return;

        bool result = Completed.GameManager.instance.AddItem(type);

        //アイテムの追加に成功したら自身を削除する
        if (result) Destroy(gameObject);
    }
}
