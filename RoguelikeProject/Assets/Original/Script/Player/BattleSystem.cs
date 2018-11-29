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
    public int Battle(Status receiver)
    {
        return Battle(receiver, status);
    }

    //2つのステータスからバトルさせる
    public int Battle(Status receiver,Status attacker)
    {
        //アタッカーの攻撃力からダメージを算出(ダメージが0未満になるのは防ぐ)
        int damage = Mathf.Max(attacker.Attack - receiver.Defense, 0);

        //リシーバーにダメージを与える
        receiver.CurrentHp -= damage;

        return damage;
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
