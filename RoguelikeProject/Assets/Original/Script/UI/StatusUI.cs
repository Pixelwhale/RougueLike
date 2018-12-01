using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField]
    private Text hpText, atkText, defText, foodText;

    //テキストのデフォルトの文字列
    private string hpdefault, atkdefault, defdefault, fooddefault;

    private Status playerStatus;
    private RemakePlayer player;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        playerStatus = playerObj.GetComponent<Status>();
        player = playerObj.GetComponent<RemakePlayer>();

        hpdefault = hpText.text;
        atkdefault = atkText.text;
        defdefault = defText.text;
        fooddefault = foodText.text;
    }

    private void Update()
    {
        SetHP(playerStatus.CurrentHp);
        SetAtk(playerStatus.Attack);
        SetDef(playerStatus.Defense);
        SetFood(player.Food);
    }

    private void SetHP(int hp)
    {
        hpText.text = hpdefault + hp;
    }

    private void SetAtk(int atk)
    {
        atkText.text = atkdefault + atk;
    }

    private void SetDef(int dfc)
    {
        defText.text = defdefault + dfc;
    }

    private void SetFood(int food)
    {
        foodText.text = fooddefault + food;
    }
}
