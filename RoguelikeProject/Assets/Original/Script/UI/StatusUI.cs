using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField]
    private Text hpText, atkText, foodText;

    //テキストのデフォルトの文字列
    private string hpdefault, atkdefault, fooddefault;

    private Status playerStatus;
    private RemakePlayer player;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        playerStatus = playerObj.GetComponent<Status>();
        player = playerObj.GetComponent<RemakePlayer>();

        hpdefault = hpText.text;
        atkdefault = atkText.text;
        fooddefault = foodText.text;
    }

    private void Update()
    {
        SetHP(playerStatus.CurrentHp);
        SetAtk(playerStatus.Attack);
        SetFood(player.Food);
    }

    public void SetHP(int hp)
    {
        hpText.text = hpdefault + hp;
    }

    public void SetAtk(int atk)
    {
        atkText.text = atkdefault + atk;
    }

    public void SetFood(int food)
    {
        foodText.text = fooddefault + food;
    }
}
