using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    #region GAME_INFO
    [SerializeField]
    private int life;
    public int Life
    {
        get { return life; }
        set 
        { 
            life = value;
            if (infoManager != null)
            {
                infoManager.lifeText.text = life.ToString();
            }
            if (life == 0)
            {
                //Game over
                GameOver();
            }
        }
    }
    [SerializeField]
    private string mode;
    public string Mode
    {
        get { return mode; }
        set { mode = value; }
    }
    private int ratio;
    public int Ratio
    {
        get { return ratio; }
        set { ratio = value; }
    }
    private int totalKills;
    public int TotalKills
    {
        get { return totalKills; }
        set 
        {
            totalKills = value; 
            if(infoManager != null)
            {
                infoManager.killsText.text = $"{totalKills.ToString()} kills";
            }
        }
    }
    private int end;
    public int End
    {
        get { return end; }
        set 
        {
            end = value;
        }
    }
    [SerializeField]
    private float preTime;
    public float PreTime
    {
        get { return preTime; }
        set { preTime = value; }
    }
    private int upgradeBaseGem;
    public int UpgradeBaseGem
    {
        get { return upgradeBaseGem; }
    }
    private int minRandomGem;
    public int MinRandomGem
    {
        get { return minRandomGem; }
    }
    private int maxRandomGem;
    public int MaxRandomGem
    {
        get { return maxRandomGem; }
    }
    #endregion
    #region ROUND_INFO
    [SerializeField]
    int rounds;
    public int Rounds
    {
        get { return rounds; }
        set
        {
            rounds = value;
            if (infoManager != null)
            {
                if (End == 0)
                {
                    infoManager.roundText.text = $"{rounds.ToString()} Round";
                }
                else if(rounds <= End)
                {
                    // Clear
                    //Debug.Log($"{rounds}/{End}");
                    infoManager.roundText.text = $"{rounds.ToString()} Round";
                }                    
            }
        }
    }
    public int unitsPerRound;
    [SerializeField]
    int removedEnemyCnt;
    public int RemovedEnemyCnt
    {
        get { return removedEnemyCnt; }
        set
        {
            //Debug.Log(removedEnemyCnt);
            removedEnemyCnt = value;
            if (removedEnemyCnt == unitsPerRound)
            {

                IsRoundClear = true;

            }
        }
    }
    [SerializeField]
    bool isRoundClear = true;
    public bool IsRoundClear
    {
        get { return isRoundClear; }
        set
        {
            isRoundClear = value;
            if (isRoundClear)
            {
                Gold += 300; 
                removedEnemyCnt = 0;
                //Rounds++;
            }
        }
    }
    #endregion
    #region CREDITS
    [SerializeField]
    int gold;
    public int Gold
    {
        get { return gold; }
        set 
        {
            gold = value;
            if (creditManager != null)
            {
                creditManager.SetGold();
            }
        }
    }
    [SerializeField]
    int gem;
    public int Gem
    {
        get { return gem; }
        set 
        { 
            gem = value;
            if (creditManager != null)
            {
                creditManager.SetGem();
            }
        }
    }

    int crystals;
    public int Crystals
    {
        get { return crystals; }
        set
        {
            crystals = value;
            if (creditManager != null)
            {
                creditManager.SetCrystal();
            }
        }
    }
    #endregion
    public enum Selected
    {
        NONE, TOWER_AREA, TOWER, ENEMY
    }
    public Selected selected;
    [SerializeField]

    private GameObject selectedObject;
    public GameObject SelectedObject
    {
        get { return selectedObject; }
        set
        {
            selectedObject = value;
            ButtonManager.Instance.MainBtn.onClick.RemoveAllListeners();
            if (selectedObject == null)
            {
                selected = Selected.NONE;
                if (statusController != null)
                {
                    statusController.gameObject.SetActive(false);
                }
            }
            else
            {
                statusController.gameObject.SetActive(true);
                switch (selectedObject.layer)
                {
                    case 6:
                        //Debug.Log("TowerArea");
                        selected = Selected.TOWER_AREA;
                        ButtonManager.Instance.MainBtn.onClick.AddListener(() => ButtonManager.Instance.OnBuildBtnClicked());
                        statusController.gameObject.SetActive(false);
                        break;
                    case 7:
                        //Debug.Log("Tower");
                        selected = Selected.TOWER;
                        ButtonManager.Instance.MainBtn.onClick.AddListener(() => ButtonManager.Instance.OnMergeBtnClicked(selectedObject));
                        selectedObject.GetComponent<TowerUIController>().IndicatesTower(true);
                        statusController.DisplayTower();
                        break;
                    case 8:
                        selected = Selected.ENEMY;
                        statusController.DisplayEnemy();
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private CreditManager creditManager;
    private GameInfoManager infoManager;
    [SerializeField]
    private StatusController statusController;
    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);


        // test pretime
        PreTime = 5f;
        IsRoundClear = true;
        selected = Selected.NONE;

        unitsPerRound = 20;

        minRandomGem = 20;
        maxRandomGem = 120;
        upgradeBaseGem = 10;

        // playscene에서 생성되었다 사라지는 GameManager에 의한 데이터 변동 방지
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }
    public void InitGameData(string modeName, int gold, int life, int ratio, int end)
    {
        mode = modeName;
        this.gold = gold;
        this.life = life;
        this.ratio = ratio;
        this.end = end;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 1)
        {
            creditManager = GameObject.FindGameObjectWithTag("CreditManager").GetComponent<CreditManager>();
            creditManager.SetGold();
            creditManager.SetGem();
            infoManager = GameObject.FindGameObjectWithTag("InfoManager").GetComponent<GameInfoManager>();
            statusController = GameObject.FindGameObjectWithTag("Status").GetComponent<StatusController>();
            //Debug.Log(life);
            infoManager.lifeText.text = life.ToString();
            Rounds = 1;
            Life = life;
            Gold = gold;
            Ratio = ratio;
            End = end;
            Mode = this.mode;
            SelectedObject = null;
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over");
    }
}
