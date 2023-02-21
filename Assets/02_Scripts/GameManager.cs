using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        set { life = value; }
    }
    [SerializeField]
    private string difficulty;
    public string DifficultyName
    {
        get { return difficulty; }
        set { difficulty = value; }
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
        set { totalKills = value; }
    }
    [SerializeField]
    private float preTime;
    public float PreTime
    {
        get { return preTime; }
        set { preTime = value; }
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
        }
    }
    int removedEnemyCnt;
    public int RemovedEnemyCnt
    {
        get { return removedEnemyCnt; }
        set
        {
            //Debug.Log(removedEnemyCnt);
            removedEnemyCnt = value;
            if(removedEnemyCnt == 20)
            {
                removedEnemyCnt = 0;
                isRoundClear = true;
                gold += 300;
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
        }
    }
    #endregion
    #region CREDITS
    [SerializeField]
    int gold;
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }
    int gem;
    public int Gem
    {
        get { return gem; }
        set { gem = value; }
    }
    #endregion
    public enum Selected
    {
        NONE, TOWER_AREA, TOWER, ENEMY
    }
    public Selected selected;
    public GameObject selectedObject;

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
        rounds = 1;
        //ratio = 1;

        // test pretime
        PreTime = 5f;
        IsRoundClear = true;
        selected = Selected.NONE;
        selectedObject = null;
        //RemovedEnemyCnt = 0;
    }
}
