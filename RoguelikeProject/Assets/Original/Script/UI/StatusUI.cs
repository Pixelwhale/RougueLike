using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    private int hp = 10;
    private int atk = 100;
    private int food = 1000;

    public void SetHP(int hp)
    {
        this.hp = hp;
		transform.Find("hp").GetComponent<Text>().text =   "HP   : " + hp;
    }

    public void SetAtk(int atk)
    {
        this.atk = atk;
		transform.Find("atk").GetComponent<Text>().text =  "ATK  : " + atk;
    }

    public void SetFood(int food)
    {
        this.food = food;
		transform.Find("food").GetComponent<Text>().text = "FOOD : " + food;
    }
}
