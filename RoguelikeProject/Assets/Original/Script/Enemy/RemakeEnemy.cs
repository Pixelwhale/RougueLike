using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Completed;

public class RemakeEnemy : MovingObject
{
    //攻撃した時の音1
    public AudioClip attackSound1;

    //攻撃した時の音2
    public AudioClip attackSound2;

    [SerializeField]
    private GameObject lifetextPrefab;

    //自身のAnimator
    private Animator animator;

    //ターゲットとなるプレイヤーのtransform
    private Transform target;

    //移動をスキップ(飛ばす)するかどうか
    private bool skipMove;

    private BattleSystem battleSystem;

    private LifeText playerlife;

    //overrideしたStart
    protected override void Start()
    {
        //自身をGameManagerに登録
        GameManager.instance.AddEnemyToList(this);

        animator = GetComponent<Animator>();
        battleSystem = GetComponent<BattleSystem>();

        //targetをPlayerのtransformに設定
        target = GameObject.FindGameObjectWithTag("Player").transform;

        skipMove = false;

        //GameObject lifeTextObj = Instantiate(lifetextPrefab);
        //lifeTextObj.transform.parent = GameObject.Find("LifeTextManager").transform;
        //myLifeText = lifeTextObj.GetComponent<LifeText>();
        //myLifeText.LifeOwner = gameObject;

        playerlife = target.GetComponentInChildren<LifeText>();

        base.Start();
    }

    //移動量に応じた移動処理と移動時に必要な処理を呼ぶ
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //移動をスキップさせる状態だったら
        if (skipMove)
        {
            //スキップさせない状態にして
            skipMove = false;
            //今回は移動をスキップ
            return;

        }

        //ローグライク的な移動処理
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        //Enemyが移動できなかった時は hit != null
        if (!Move(xDir, yDir, out hit))
        {
            HitProcess(hit);
        }

        //今回移動したので次回は移動をスキップさせることにする
        skipMove = true;
    }

    //移動処理
    public void MoveEnemy()
    {
        //移動量をそれぞれ0で初期化
        int xDir = 0;
        int yDir = 0;

        //自身とプレイヤーのpositionのxを比べてほぼ同じ位置にいたらy方向の移動にする
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)

            //y方向の移動量を設定
            yDir = target.position.y > transform.position.y ? 1 : -1;

        //x方向の移動にする
        else
            //x方向の移動量を設定
            xDir = target.position.x > transform.position.x ? 1 : -1;

        //Playerに対する移動を決定
        AttemptMove<RemakePlayer>(xDir, yDir);
    }

    //プレイヤーに当たって移動できなかった時の処理
    protected override void OnCantMove<T>(T component) { }

    //当たった相手がいた時の処理
    private void HitProcess(RaycastHit2D hit)
    {
        switch (hit.collider.gameObject.tag)
        {
            //Playerと当たった時の処理
            case "Player":
                HitPlayer(hit.collider.gameObject);
                break;

            //その他はbreakする
            default:
                break;
        }
    }

    //自身(Enemy)とPlayerが当たった時の処理
    private void HitPlayer(GameObject player)
    {
        //Playerのステータスを取得
        Status receiver = player.GetComponent<Status>();

        int damage = 0;

        //バトルさせる
        damage = battleSystem.Battle(receiver);

        //攻撃のアニメーションを再生
        animator.SetTrigger("enemyAttack");

        //攻撃した音を再生
        SoundManager.instance.RandomizeSfx(attackSound1, attackSound2);

        //プレイヤーのライフテキストに書き込みを行う
        playerlife.CallDamageText(damage);
    }
}
