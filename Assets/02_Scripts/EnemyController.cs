using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameManager gm;

    Transform enemyTr;
    public float moveSpeed = 0.45f;

    GameObject spawn;
    GameObject arrival;

    Transform[] turnPoints;
    Vector3 dir;
    int tpIndex = 0;

    public float hp = 100f;
    public float initHp = 100f;
    public float hpPercent = 1.25f;
    void Start()
    {
        gm = GameManager.Instance;

        spawn = GameObject.FindGameObjectWithTag("Spawn");
        arrival = GameObject.FindGameObjectWithTag("Arrival");

        //MoveToArrival();
    }
    private void Update()
    {
        if (hp <= 0)
        {
            EnemyDead();
        }
    }
    public void MoveToArrival()
    {
        enemyTr = GetComponent<Transform>();
        GameObject[] tps = GameObject.FindGameObjectsWithTag("TurnPoints");
        turnPoints = new Transform[tps.Length];
        for (int i = 0; i < tps.Length; i++)
        {
            turnPoints[i] = tps[i].GetComponent<Transform>();
        }
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            if (tpIndex < turnPoints.Length)
            {
                dir = (turnPoints[tpIndex].position - enemyTr.position);
                enemyTr.Translate(dir.normalized * moveSpeed * Time.deltaTime);
                if (dir.magnitude < Vector3.forward.magnitude * 0.1)
                {
                    enemyTr.position = turnPoints[tpIndex].position;
                    dir = (turnPoints[tpIndex].position - enemyTr.position);
                    // 회전 추가
                    tpIndex++;
                }
            }
            else
            {
                dir = (arrival.transform.position - enemyTr.position);
                enemyTr.Translate(dir.normalized * moveSpeed * Time.deltaTime);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrival"))
        {
            StopCoroutine(MoveCoroutine());
            InitEnemy();
        }
    }

    public void EnemyDead()
    {
        StopCoroutine(MoveCoroutine());
        ++gm.RemovedEnemyCnt;
        //Debug.Log($"removed: {gm.RemovedEnemyCnt}");
        ++gm.TotalKills;
        //Debug.Log(gm.TotalKills);
        InitEnemy();
    }

    public void InitEnemy()
    {
        // 렌더러 끄기
        int round = gm.Rounds + 1;
        gameObject.name = $"Round {round}";
        enemyTr.position = spawn.transform.position;
        tpIndex = 0;
        gameObject.SetActive(false);
        // 라운드에 따른 hp 상승 및 초기화      
        if(round%5==1 && round != 1)
        {
            initHp *= hpPercent;
        }
        hp = initHp * round * gm.Ratio;
    }
}
