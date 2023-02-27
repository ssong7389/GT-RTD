using UnityEngine;
using System;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    private static TowerManager _instance;

    public static TowerManager Instance
    {
        get { return _instance; }
    }

    public enum Ranks
    {
        NORMAL, RARE, HERO, LEGEND, GOD
    }
    public enum Type
    {
        HUMAN, SUIN, DEMON
    }
    [SerializeField]
    private int[] upgrades;
    public int[] Upgrades
    {
        get { return upgrades; }
        set
        {
            upgrades = value;

        }
    }

    GameManager gm;

    int towerPrice;
    public List<GameObject> towerSet;

    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        //int rankLength = Enum.GetValues(typeof(Ranks)).Length;
        int typeLength = Enum.GetValues(typeof(Type)).Length;
        upgrades = new int[typeLength];

        // Load data from csv
        List<Dictionary<string, object>> data = CSVReader.Read("Towers");        
        for (var i = 0; i < data.Count; i++)
        {
            towerSet.Add(InitTower(data[i]));
        }
    }
    private void Start()
    {
        gm = GameManager.Instance;
        towerPrice = 100;
    }
 
    public void BuildTower(Transform buildArea)
    {
        if(gm.Gold >= towerPrice)
        {
            gm.Gold -= towerPrice;

            Vector3 pos = buildArea.position - Vector3.up * 0.25f;
            GameObject tower =  GetRandomTower(Ranks.NORMAL, pos);
        }
    }
    public void SellTower(GameObject towerToSell)
    {
        int price = towerPrice * (1 << (int)towerToSell.GetComponent<Tower>().Rank);
        gm.Gold += price / 2;
        towerToSell.GetComponent<CharacterShadow>().IndicatesTower(false);
        Destroy(towerToSell);
    }

    public void MergeTower(GameObject selectedTower)
    {
        Ranks rank = selectedTower.GetComponent<Tower>().Rank;
        //Debug.Log($"Merge rank: {rank}");
        if (rank == Ranks.GOD)
        {
            return;
        }
        GameObject[] rankTowers = GameObject.FindGameObjectsWithTag(rank.ToString());
        int index = Array.IndexOf(rankTowers, selectedTower);
        Transform selectedTr = selectedTower.GetComponent<Transform>();
        foreach (var tower in rankTowers)
        {
            if (tower.name == selectedTower.name)
            {
                if (Array.IndexOf(rankTowers, tower) != index)
                {
                    // selectedTr에 다음 등급 타워 생성
                    GameObject mergedTower = GetRandomTower(++rank, selectedTr.position);
                    selectedTower.GetComponent<CharacterShadow>().IndicatesTower(false);
                    //GameManager.Instance.SelectedObject = null;
                    Destroy(selectedTower);
                    Destroy(tower);
                    break;
                }
            }
        }
        
    }
    private GameObject GetRandomTower(Ranks rank, Vector3 pos)
    {
        List<GameObject> towerList = towerSet.FindAll(tower => tower.GetComponent<Tower>().Rank == rank);

        GameObject tower = Instantiate(towerList[UnityEngine.Random.Range(0, towerList.Count)]);
        tower.name = tower.GetComponent<Tower>().TowerName;
        tower.transform.position = pos;
        return tower;
    }

    private GameObject InitTower(Dictionary<string, object> data)
    {
        GameObject towerPrefab = Resources.Load<GameObject>((string)data["Path"]);
        Tower towerData = towerPrefab.GetComponent<Tower>();

        Ranks rank = (Ranks)data["Rank"];
        //Debug.Log(rank);
        Type type = (Type)data["Type"];
        //Debug.Log(type);
        float dmg = (float)data["Damage"];
        float increment = (float)data["Increment"];
        float range = (float)data["Range"];
        float speed = (float)data["Speed"];
        string towerName = (string)data["TowerName"];
        string assetName = (string)data["AssetName"];
        string weaponAtlas = (string)data["WeaponAtlas"];
        string weaponType = (string)data["WeaponType"];
        string attackName = (string)data["AttackName"];
        towerData.InitTowerData(rank, type, dmg, increment, range, speed);
        towerData.InitTowerAssets(towerName, assetName, weaponAtlas, weaponType, attackName);
        towerData.InitTowerSkeleton();

        return towerPrefab;
    }
    public int GetUpgrade(Type type)
    {
        return upgrades[(int)type];
    }
}
