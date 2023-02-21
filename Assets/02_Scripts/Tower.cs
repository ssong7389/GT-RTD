using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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
    enum Dir
    {
        UP, DOWN, SIDE
    }
    Dir dir;
    Transform towerTr;
    public GameObject[] directions = new GameObject[3];
    GameObject activatedDir;
    [SerializeField]
    GameObject target;
    public void SetupTower(TowerManager.Ranks rank, TowerManager.Type type, float dmg, float increment,float range,float attackSpeed,string towerName)
    {
        this.rank = rank;
        this.type = type;
        this.dmg = dmg;
        this.attackSpeed = attackSpeed;
        this.range = range;
        this.increment = increment;
        this.towerName = towerName;
    }

    private void Start()
    {
        tm = TowerManager.Instance;
        //gm = GameManager.Instance;
        towerTr = GetComponent<Transform>();
        directions = new GameObject[towerTr.childCount];
        // back side front
        for (int i = 0; i < towerTr.childCount; i++)
        {
            directions[i] = towerTr.GetChild(i).gameObject;
            directions[i].SetActive(false);
        }
        activatedDir = directions[(int)Dir.DOWN];
        activatedDir.SetActive(true);

    }
    private void Update()
    {
        SetTargetAndAttack();
        if (target != null)
        {
            //Debug.Log(Quaternion.FromToRotation(Vector2.one, (target.transform.position - towerTr.position)).eulerAngles.z);
            float angle = Quaternion.FromToRotation((target.transform.position - towerTr.position), new Vector2(-1, 1)).eulerAngles.z;
            activatedDir.SetActive(false);
            if (angle > 0 && angle <= 90)
            {
                activatedDir = directions[(int)Dir.UP];
            }
            else if(angle >90 && angle <= 180)
            {
                activatedDir = directions[(int)Dir.SIDE];
                towerTr.localScale = new Vector3(1, 1, 1);
            }
            else if(angle >180 && angle <= 270)
            {
                activatedDir = directions[(int)Dir.DOWN];
            }
            else if(angle>270 && angle <= 360)
            {
                activatedDir = directions[(int)Dir.SIDE];
                towerTr.localScale = new Vector3(-1, 1, 1);
            }
            activatedDir.SetActive(true);
        }
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
