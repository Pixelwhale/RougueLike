using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTextData : MonoBehaviour
{
    [Header("回復した時のテキストの色")]
    public Color healColor = Color.green;

    [Header("ダメージを受けた時のテキストの色")]
    public Color damageColor = Color.red;

    [Header("テキストの動く移動量")]
    public Vector2 moveVelocity;

    [Header("プレイヤーの位置に対して出現位置をどのくらい変更するか")]
    public Vector2 appearShift;

    [Header("テキストの移動にかかる時間")]
    public float moveTime = 2f;
}
