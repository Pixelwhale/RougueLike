using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    //自身のステータスComponent
    private Status status;

	void Start ()
    {
        status = GetComponent<Status>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    //アタッカーは自身のものを設定
    public void Battle(Status receiver)
    {
        Battle(receiver, status);
    }

    //2つのステータスからバトルさせる
    public void Battle(Status receiver,Status attacker)
    {
        //アタッカーの攻撃力からダメージを算出(ダメージが0未満になるのは防ぐ)
        int damage = Mathf.Max(attacker.Attack - receiver.Defense, 0);

        //リシーバーにダメージを与える
        receiver.CurrentHp -= damage;
    }

    public bool IsDead()
    {
        return IsDead(status);
    }

    public bool IsDead(Status status)
    {
        return status.CurrentHp <= 0;
    }
}
