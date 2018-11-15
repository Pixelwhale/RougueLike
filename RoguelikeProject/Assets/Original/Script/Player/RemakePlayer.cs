using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Completed;

public class RemakePlayer : MovingObject
{
    public float restartLevelDelay = 1f;        //階層が変わる階段に当たった時に、Restartを呼ぶまでの時間(秒)
    public int pointsPerFood = 10;              //フードに当たった時の回復量
    public int pointsPerSoda = 20;              //ソーダに当たった時の回復量
    public int wallDamage = 1;                  //壁にもHPがあるので、壁に攻撃したとき(壁に向かって移動しようとすることで攻撃したことになる)壁に与えるダメージ量
    public Text foodText;                       //満腹度を表示するためのTextUI
    public AudioClip moveSound1;                //プレイヤーが動いた時の音1
    public AudioClip moveSound2;                //プレイヤーが動いた時の音2
    public AudioClip eatSound1;                 //プレイヤーがフードを食べた時の音1
    public AudioClip eatSound2;                 //プレイヤーがフードを食べた時の音2
    public AudioClip drinkSound1;               //プレイヤーがソーダを飲んだ時の音1
    public AudioClip drinkSound2;               //プレイヤーがソーダを飲んだ時の音2
    public AudioClip gameOverSound;             //ゲームオーバーになったと時の音

    private Animator animator;                  //プレイヤーのアニメーター
    private int food;                           //現在の満腹度

    //自身の戦闘を扱うスクリプト
    private BattleSystem battleSystem;

    private Status status;
    
    //overrideしたStart
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        battleSystem = GetComponent<BattleSystem>();
        status = GetComponent<Status>();

        //満腹度を初期化
        food = GameManager.instance.playerFoodPoints;

        //満腹度をTextUIに記載
        foodText.text = "Food: " + food + AddTestHP();

        //継承元のStartを呼ぶ
        base.Start();
    }

    //GameObjectが非アクティブ、または削除されるときの呼ばれる
    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    //更新
    private void Update()
    {
        //満腹度をTextUIに反映
        foodText.text = " Food: " + food + AddTestHP();

        //プレイヤーのターンでなかったらreturn
        if (!GameManager.instance.playersTurn) return;

        //横方向、縦方向の入力量
        int horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        int vertical = (int)(Input.GetAxisRaw("Vertical"));

        //横方向の入力があったら縦方向の入力は受け付けない
        if (horizontal != 0)
        {
            vertical = 0;
        }

        //横、縦どちらかの入力があったら
        if (horizontal != 0 || vertical != 0)
        {
            //移動量に応じた移動処理をする
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    //移動量に応じた移動処理と移動時に必要な処理を呼ぶ
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //満腹度を減らす
        food--;

        //満腹度をTextUIに反映させる
        foodText.text = "Food: " + food + AddTestHP();

        //継承元のローグライク的な移動処理を行う
        base.AttemptMove<T>(xDir, yDir);

        //移動する方向に出したRayにHitした対象
        RaycastHit2D hit;

        //移動が成功したらtrue(成功の場合は hit == null)
        if (Move(xDir, yDir, out hit))
        {
            //移動した時の音を再生
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        //移動に失敗したら誰かに当たっている
        else
        {
            HitProcess(hit);
        }

        //GameOverしてないかチェック
        CheckIfGameOver();

        //プレイヤーのターンを終了する
        GameManager.instance.playersTurn = false;
    }

    //移動入力がされたが移動できなかった時の処理
    protected override void OnCantMove<T>(T component)
    {
        //プレイヤーが移動できなかった時の相手はWallなので、Wallにキャスト
        Wall wall = component as Wall;

        //壁にダメージを与える
        wall.DamageWall(wallDamage);

        //攻撃(壁に向かって移動入力することで攻撃したことになる)した際のアニメーションを再生
        animator.SetTrigger("playerChop");
    }

    //TriggerEnter
    private void OnTriggerEnter2D(Collider2D other)
    {
        //相手が階層を抜ける階段だったら
        if (other.tag == "Exit")
        {
            //自身のRestartメソッドをrestartLevelDelay秒後に呼ぶ
            Invoke("Restart", restartLevelDelay);

            //自身のスクリプトを一旦止める
            enabled = false;
        }

        //相手がフードだったら
        else if (other.tag == "Food")
        {
            //満腹度を回復させる
            food += pointsPerFood;

            //満腹度をTextUIに記載
            foodText.text = "+" + pointsPerFood + " Food: " + food + AddTestHP();

            //フードを食べた音を再生
            SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);

            //フードを非アクティブにする
            other.gameObject.SetActive(false);
        }

        //相手がソーダだったら
        else if (other.tag == "Soda")
        {
            //満腹度を回復させる
            food += pointsPerSoda;

            //満腹度をTextUIに記載
            foodText.text = "+" + pointsPerSoda + " Food: " + food + AddTestHP();

            //ソーダを飲んだ時の音を再生
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);

            //ソーダを非アクティブにする
            other.gameObject.SetActive(false);
        }
    }

    //プレイヤーが階段に当たり階層が変わった時に呼ばれる処理
    private void Restart()
    {
        //シーンをロードする(全てのロードされているシーンを閉じ、指定のシーンだけロード)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    //移動以外で満腹度を減らされる処理
    public void LoseFood(int loss)
    {
        //ヒットした際のアニメーションを再生
        animator.SetTrigger("playerHit");

        //満腹度を減らす
        food -= loss;

        //満腹度をTextUIに反映
        foodText.text = "-" + loss + " Food: " + food + AddTestHP();

        //GameOverをチェックする
        CheckIfGameOver();
    }

    //GameOverをチェックする
    private void CheckIfGameOver()
    {
        //満腹度が0以下だったらGameOverの処理をする
        if (food <= 0)
        {
            //GameOverの音を再生する
            //SoundManager.instance.PlaySingle(gameOverSound);

            //BGMを止める
            //SoundManager.instance.musicSource.Stop();

            //GameManagerのGameOverを呼ぶ
            //GameManager.instance.GameOver();
        }
    }

    //当たった相手がいた時の処理
    private void HitProcess(RaycastHit2D hit)
    {
        switch (hit.collider.gameObject.tag)
        {
            //Enemyと当たった時の処理
            case "Enemy":
                HitEnemy(hit.collider.gameObject);
                break;

            //その他はbreakする
            default:
                break;
        }
    }

    private void HitEnemy(GameObject enemy)
    {
        //相手がenemyだったら
        Status receiver = enemy.GetComponent<Status>();

        //自身とEnemyをバトルさせる
        battleSystem.Battle(receiver);

        //攻撃した際のアニメーションを再生
        animator.SetTrigger("playerChop");
    }

    private string AddTestHP()
    {
        return "\nHP:" + status.CurrentHp;
    }
}
