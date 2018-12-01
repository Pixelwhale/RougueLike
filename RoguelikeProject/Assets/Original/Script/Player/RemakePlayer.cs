using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Completed;

public class RemakePlayer : MovingObject
{
    public float restartLevelDelay = 1f;        //階層が変わる階段に当たった時に、Restartを呼ぶまでの時間(秒)
    public int wallDamage = 1;                  //壁にもHPがあるので、壁に攻撃したとき(壁に向かって移動しようとすることで攻撃したことになる)壁に与えるダメージ量
    public AudioClip moveSound1;                //プレイヤーが動いた時の音1
    public AudioClip moveSound2;                //プレイヤーが動いた時の音2
    public AudioClip gameOverSound;             //ゲームオーバーになったと時の音

    [SerializeField,Header("空腹時の1歩のダメージ量")]
    private int emptyfoodDamgage = 5;

    private Animator animator;                  //プレイヤーのアニメーター
    private int food;                           //現在の満腹度

    //自身の戦闘を扱うスクリプト
    private BattleSystem battleSystem;

    private Status status;
    private LifeText lifetext;
    private bool isGameend;

    public LifeText LifeText
    {
        get { return lifetext; }
    }

    public int Food
    {
        get { return food; }
    }
    
    //overrideしたStart
    protected override void Start()
    {
        animator = GetComponent<Animator>();
        battleSystem = GetComponent<BattleSystem>();
        status = GetComponent<Status>();
        lifetext = GetComponentInChildren<LifeText>();

        //満腹度を初期化
        food = GameManager.instance.playerFoodPoints;
        status.CurrentHp = GameManager.instance.playerHP;

        isGameend = false;

        //継承元のStartを呼ぶ
        base.Start();
    }

    //GameObjectが非アクティブ、または削除されるときの呼ばれる
    private void OnDisable()
    {
        if (isGameend) return;

        //プレイヤーの満腹度、HPを保存
        GameManager.instance.playerFoodPoints = food;
        GameManager.instance.playerHP = status.CurrentHp;
    }

    //更新
    private void Update()
    {
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

        //GameOverしてないかチェック
        CheckIfGameOver();
    }

    //移動量に応じた移動処理と移動時に必要な処理を呼ぶ
    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        //満腹度を減らす
        ReduceFood();

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
            StartCoroutine(utility.Coroutine.DelayMethod(restartLevelDelay, GameManager.Restart));

            //自身のスクリプトを一旦止める
            enabled = false;
        }
    }

    //プレイヤーが階段に当たり階層が変わった時に呼ばれる処理
    private void Restart()
    {
        //シーンをロードする(全てのロードされているシーンを閉じ、指定のシーンだけロード)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    private void ReduceFood()
    {
        food--;

        if (food < 0)
        {
            status.CurrentHp -= emptyfoodDamgage;
        }

        food = Mathf.Max(0, food);
    }

    //GameOverをチェックする
    private void CheckIfGameOver()
    {
        if (isGameend) return;

        //HPが0以下だったらGameOverの処理をする
        if (status.IsDead)
        {
            //GameOverの音を再生する
            SoundManager.instance.PlaySingle(gameOverSound);

            //BGMを止める
            SoundManager.instance.musicSource.Stop();

            //GameManagerのGameOverを呼ぶ
            GameManager.instance.GameOver();

            isGameend = true;

            enabled = false;
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

        int damage = 0;

        //自身とEnemyをバトルさせる
        damage = battleSystem.Battle(receiver);

        //攻撃した際のアニメーションを再生
        animator.SetTrigger("playerChop");

        //enemyのlifeTextを取得
        LifeText lifeText = enemy.GetComponentInChildren<LifeText>();//.LifeText;
        lifeText.CallDamageText(damage);

        if (receiver.IsDead)
        {
            Destroy(enemy);
        }
    }

    public void RecoveryHP(int hp)
    {
        status.CurrentHp += hp;
        lifetext.CallHealText(hp);
    }

    public void EatFood(int food)
    {
        this.food += food;
        lifetext.CallEatText(food);
    }
}
