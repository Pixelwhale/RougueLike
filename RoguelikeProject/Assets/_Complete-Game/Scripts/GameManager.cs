using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Completed
{
    using System.Collections.Generic;       //Allows us to use Lists. 
    using UnityEngine.UI;                   //Allows us to use UI.

    public class GameManager : MonoBehaviour
    {
        struct GameData
        {
            public int playerFood;
            public int playerHP;
            public int level;
        }

        public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.
        public float turnDelay = 0.1f;                          //Delay between each Player turn.
        public int playerFoodPoints = 100;                      //Starting value for Player food points.
        public int playerHP = 100;
        public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
        [HideInInspector]
        public bool playersTurn = true;     //Boolean to check if it's players turn, hidden in inspector but public.


        private Text levelText;                                 //Text to display current level number.
        private GameObject levelImage;                          //Image to block out level as levels are being set up, background for levelText.
        private RemakeBoardManager boardScript;                     //Store a reference to our BoardManager which will set up the level.
        private int level = 1;                                  //Current level number, expressed in game as "Day 1".
        private List<RemakeEnemy> enemies;                          //List of all Enemy units, used to issue them move commands.
        private bool enemiesMoving;                             //Boolean to check if enemies are moving.
        private bool doingSetup = true;                         //Boolean to check if we're setting up board, prevent Player from moving during setup.
        private GameData initData;

        //装備アイテムのリスト
        private ItemEqipment[] eqipmentItemList;

        //消費アイテムのリスト
        private ItemUse[] useItemList;

        private CountDownText countdown;
        private bool isRetry = false;

        public ItemEqipment[] EqipmentItemList
        {
            get { return eqipmentItemList; }
        }

        public ItemUse[] UseItemList
        {
            get { return useItemList; }
        }

        //Awake is always called before any Start functions
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)
            {
                Screen.fullScreen = true;
                //if not, set instance to this
                instance = this;
                initData.playerFood = playerFoodPoints;
                initData.playerHP = playerHP;
                initData.level = level;
                GameStart();
            }
            //If instance already exists and it's not this:
            else if (instance != this)
            {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            //Assign enemies to a new List of Enemy objects.
            enemies = new List<RemakeEnemy>();

            //Get a component reference to the attached BoardManager script
            boardScript = GetComponent<RemakeBoardManager>();

            isRetry = false;

            //Call the InitGame function to initialize the first level 
            InitGame();
        }

        //this is called only once, and the paramter tell it to be called only after the scene was loaded
        //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization()
        {
            //register the callback to be called everytime the scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //This is called each time a scene is loaded.
        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (instance.isRetry)
            {
                instance.isRetry = false;
            }
            else
            {
                instance.level++;
            }

            instance.InitGame();
        }


        //Initializes the game for each level.
        void InitGame()
        {
            //While doingSetup is true the player can't move, prevent player from moving while title card is up.
            doingSetup = true;

            //Get a reference to our image LevelImage by finding it by name.
            levelImage = GameObject.Find("LevelImage");

            //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
            levelText = GameObject.Find("LevelText").GetComponent<Text>();

            //Set the text of levelText to the string "Day" and append the current level number.
            levelText.text = "Day " + level;

            //Set levelImage to active blocking player's view of the game board during setup.
            levelImage.SetActive(true);

            //Call the HideLevelImage function with a delay in seconds of levelStartDelay.
            Invoke("HideLevelImage", levelStartDelay);

            //Clear any Enemy objects in our List to prepare for next level.
            enemies.Clear();

            //Call the SetupScene function of the BoardManager script, pass it current level number.
            boardScript.SetupScene(level);

            countdown = GameObject.FindGameObjectWithTag("MainCanvas").transform.Find("RetryCountDown").GetComponent<CountDownText>();
            countdown.gameObject.SetActive(false);
        }


        //Hides black image used between levels
        void HideLevelImage()
        {
            //Disable the levelImage gameObject.
            levelImage.SetActive(false);

            //Set doingSetup to false allowing player to move again.
            doingSetup = false;
        }

        //Update is called every frame.
        void Update()
        {
            //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
            if (playersTurn || enemiesMoving || doingSetup)

                //If any of these are true, return and do not start MoveEnemies.
                return;

            //Start moving enemies.
            StartCoroutine(MoveEnemies());
        }

        //Call this to add the passed in Enemy to the List of Enemy objects.
        public void AddEnemyToList(RemakeEnemy script)
        {
            //Add Enemy to List enemies.
            enemies.Add(script);
        }

        public void GameStart()
        {
            playerFoodPoints = initData.playerFood;
            playerHP = initData.playerHP;
            level = initData.level;

            eqipmentItemList = new ItemEqipment[4];
            for(int i = 0; i < 4; i++)
            {
                eqipmentItemList[i].type = ItemType.NONE;
                eqipmentItemList[i].isEqipment = false;
            }

            useItemList = new ItemUse[8];
            for(int i = 0; i < 8; i++)
            {
                useItemList[i].type = ItemType.NONE;
                useItemList[i].num = 0;
            }
        }

        //GameOver is called when the player reaches 0 food points
        public void GameOver()
        {
            //Set levelText to display number of levels passed and game over message
            levelText.text = "After " + level + " days, you starved.";

            //Enable black background image gameObject.
            levelImage.SetActive(true);

            if (countdown != null)
            {
                countdown.gameObject.SetActive(true);
                countdown.TimeUpAction = GameStart;
                countdown.TimeUpAction += () => isRetry = true;
            }
        }

        public static void Restart()
        {
            //シーンをロードする(全てのロードされているシーンを閉じ、指定のシーンだけロード)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }

        //Coroutine to move enemies in sequence.
        IEnumerator MoveEnemies()
        {
            //While enemiesMoving is true player is unable to move.
            enemiesMoving = true;

            //Wait for turnDelay seconds, defaults to .1 (100 ms).
            yield return new WaitForSeconds(turnDelay);

            //If there are no enemies spawned (IE in first level):
            if (enemies.Count == 0)
            {
                //Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
                yield return new WaitForSeconds(turnDelay);
            }

            CheckDeadEnemy();

            //Loop through List of Enemy objects.
            for (int i = 0; i < enemies.Count; i++)
            {
                //Call the MoveEnemy function of Enemy at index i in the enemies List.
                enemies[i].MoveEnemy();

                //Wait for Enemy's moveTime before moving next Enemy, 
                yield return new WaitForSeconds(enemies[i].moveTime);
            }
            //Once Enemies are done moving, set playersTurn to true so player can move.
            playersTurn = true;

            //Enemies are done moving, set enemiesMoving to false.
            enemiesMoving = false;
        }

        private void CheckDeadEnemy()
        {
            enemies.RemoveAll(e => e == null);
        }

        private void AddOnly(ref ItemUse itemNum, ItemType type)
        {
            itemNum.type = type;
            itemNum.num++;
        }

        private void UseOnly(ref ItemUse itemNum)
        {
            int n = itemNum.num;
            --n;
            itemNum.num = Mathf.Max(0, n);

            if (itemNum.num <= 0)
            {
                itemNum.type = ItemType.NONE;
            }
        }

        //配列に空の要素があるかどうか
        private bool IsEmptyItem(ItemUse[] array)
        {
            foreach (var info in array)
            {
                if (info.type == ItemType.NONE) return true;
            }

            //全てNONEでなかったのでアイテムリストが空でないからfalse
            return false;
        }
        private bool IsEmptyItem(ItemEqipment[] array)
        {
            foreach(var info in array)
            {
                if (info.type == ItemType.NONE) return true;
            }

            return false;
        }

        private int IsIndexOf(ItemUse[] array, System.Func<ItemUse, bool> condition)
        {
            for(int i = 0; i < array.Length; i++)
            {
                if (condition(array[i])) return i;
            }

            return 0;
        }
        private int IsIndexOf(ItemEqipment[] array, System.Func<ItemEqipment, bool> condition)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (condition(array[i])) return i;
            }

            return 0;
        }

        //arrayにtypeのアイテムが登録されているかどうか
        private bool IsContainsItem(ItemUse[] array, ItemType type)
        {
            foreach(var info in array)
            {
                if (info.type == type) return true;
            }

            return false;
        }

        //消費アイテムを取得したときに呼ぶ
        private bool AddUseItem(ItemType type)
        {
            bool isContains = IsContainsItem(useItemList, type);
            //アイテムに空の要素がなく、かつ指定のアイテムがすでに登録されていなかったらAdd失敗
            if (!IsEmptyItem(useItemList) && !isContains) return false;

            //同じアイテムを含んでいた場合
            if (isContains)
            {
                //同じ要素に対して追加する
                AddOnly(ref useItemList[IsIndexOf(useItemList, info => info.type == type)], type);
            }
            else
            {
                //空の要素に対して追加する
                AddOnly(ref useItemList[IsIndexOf(useItemList, info => info.type == ItemType.NONE)], type);
            }

            //Add成功
            return true;
        }

        public bool AddItem(ItemType type)
        {
            if (utility.Range.IsRangeOfInt((int)type, 0, 13))
            {
                //EQIPMENT
                return AddEqipmentItem(type);
            }

            if (utility.Range.IsRangeOfInt((int)type, 14, 16))
            {
                //USEITEM
                return AddUseItem(type);
            }

            return false;
        }

        //装備アイテムを取得したときに呼ぶ
        private bool AddEqipmentItem(ItemType type)
        {
            //空の要素がなかったらAdd失敗
            if (!IsEmptyItem(eqipmentItemList)) return false;

            //空のアイテムのアイテムタイプを更新
            eqipmentItemList[IsIndexOf(eqipmentItemList, info => info.type == ItemType.NONE)].type = type;

            //Add成功
            return true;
        }

        //消費アイテムの使用
        public void UseItem(ItemType type)
        {
            //引数のアイテムを含んでいなかったらreturn
            if (!IsContainsItem(useItemList, type)) return;

            //アイテムの消費
            UseOnly(ref useItemList[IsIndexOf(useItemList, info => info.type == type)]);
        }
    }
}

