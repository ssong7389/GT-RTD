using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Spine.Unity;
using Spine;

public class Tower : MonoBehaviour
{
    TowerManager tm;
    //GameManager gm;

    [SerializeField]
    public float dmg;
    [SerializeField]
    public float increment;

    // 2, 2.5 | 3.6, 4, 4.5 | 5.5, 6
    [SerializeField]
    float range;

    // 공격 애니메이션 길이로
    [SerializeField]
    float attackSpeed;
    [SerializeField]
    string towerName;
    public string TowerName
    {
        get { return towerName; }
    }
    public string assetName;
    // 스파인

    // 공격 이펙트
    // 타워 타입
    // 타워 등급
    [SerializeField]
    TowerManager.Ranks rank;
    [SerializeField]
    TowerManager.Type type;
    public TowerManager.Type Type
    {
        get { return type; }
    }
    public TowerManager.Ranks Rank
    {
        get { return rank; }
    }
    public enum Dir
    {
        back, front, side
    }
    Dir direction = Dir.front;
    public Dir Direction
    {
        get { return direction; }
        set
        {
            direction = value;
            path = $"characters/{assetName}/{assetName}_{direction}_SkeletonData";
            skel.skeletonDataAsset = (SkeletonDataAsset)Resources.Load(path);
            skel.Initialize(true);
        }
    }
    Transform towerTr;
    [SerializeField]
    GameObject target;
    SkeletonAnimation skel;
    string path;
    MeshRenderer meshRenderer;
    float originScaleXY;
    string weaponAtlas;
    SpineAtlasAsset items;

    public void SetupTower(TowerManager.Ranks rank, TowerManager.Type type, float dmg, float increment,
        float range,float attackSpeed,string towerName, string assetName, string weaponAtlas)
    {
        this.rank = rank;
        this.type = type;
        this.dmg = dmg;
        this.attackSpeed = attackSpeed;
        this.range = range;
        this.increment = increment;
        this.towerName = towerName;
        this.assetName = assetName;
        this.weaponAtlas = weaponAtlas;
    }

    private void Start()
    {
        tm = TowerManager.Instance;
        //gm = GameManager.Instance;
        towerTr = GetComponent<Transform>();
        skel = GetComponent<SkeletonAnimation>();
        Init();
        originScaleXY = towerTr.localScale.x;

    }
    private void Update()
    {
        SetTargetAndAttack();
        if (target != null)
        {
            //Debug.Log(Quaternion.FromToRotation(Vector2.one, (target.transform.position - towerTr.position)).eulerAngles.z);
            float angle = Quaternion.FromToRotation((target.transform.position - towerTr.position), new Vector2(-1, 1)).eulerAngles.z;
            if (angle > 0 && angle <= 90)
            {
                Direction = Dir.back;
            }
            else if(angle >90 && angle <= 180)
            {
                Direction = Dir.side;
                towerTr.localScale = new Vector3(originScaleXY, originScaleXY);
            }
            else if(angle >180 && angle <= 270)
            {
                Direction = Dir.front;
            }
            else if(angle>270 && angle <= 360)
            {
                Direction = Dir.side;
                towerTr.localScale = new Vector3(-originScaleXY, originScaleXY, 1);
            }
        }
    }
    public void Init()
    {
        tm = TowerManager.Instance;
        //gm = GameManager.Instance;
        direction = Dir.front;

        skel = GetComponent<SkeletonAnimation>();
        path = $"characters/{assetName}/{assetName}";
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = (Material)Resources.Load($"{path}_Material");
        skel.skeletonDataAsset = (SkeletonDataAsset)Resources.Load($"{path}_{direction}_SkeletonData");

        items = (SpineAtlasAsset)Resources.Load("Items/items_Atlas");
        Atlas atlas = items.GetAtlas();
        AtlasRegion item = atlas.FindRegion(weaponAtlas);

        Slot slot = skel.Skeleton.FindSlot($"[base]weapon1_{direction}");
        //slot.Attachment = item;
    }
    IEnumerator AttackCoroutine()
    {
        while (target != null)
        {
            EnemyController targetCtrl = target.GetComponent<EnemyController>();
            float distance = Vector2.Distance(towerTr.position, target.transform.position);
            if (distance > range + 0.125)
            {
                target.GetComponent<MeshRenderer>().material.color = Color.white;
                //Debug.Log(Vector2.Distance(towerTr.position, target.transform.position));
                break;
            }
            else
            {
                targetCtrl.hp -= dmg + increment * tm.GetUpgrade(type);
                yield return new WaitForSeconds(attackSpeed);
            }
            // 공격 이펙트
        }
        //Debug.Log($"target null");
        yield return new WaitForSeconds(attackSpeed);
        target = null;
        StopCoroutine(AttackCoroutine());
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    void SetTargetAndAttack()
    {
        if(target == null)
        {
            Collider[] colls = Physics.OverlapSphere(transform.position, range);
            Dictionary<GameObject, float> nearby = new Dictionary<GameObject, float>();
            foreach (var coll in colls)
            {
                if (coll.CompareTag("ENEMY"))
                {
                    nearby.Add(coll.gameObject, (coll.transform.position - towerTr.position).sqrMagnitude);
                }
            }

            if (nearby.Count == 0)
                return;

            nearby = nearby.OrderBy(near => near.Value).ToDictionary(near => near.Key, near => near.Value);

            GameObject nearest = nearby.First().Key;
            //Debug.Log($"min: {nearby.First().Value}");
            if (Vector2.Distance(towerTr.position, nearest.transform.position) < range)
            {
                target = nearest;

                // 타겟 표시..
                target.GetComponent<MeshRenderer>().material.color = Color.red;
                StartCoroutine(AttackCoroutine());
            }
        }
    }
}
