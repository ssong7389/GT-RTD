using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class EnemyController : MonoBehaviour
{
    GameManager gm;

    Transform enemyTr;
    public float moveSpeed = 0.45f;

    GameObject spawn;
    GameObject arrival;
    
    List<GameObject> turns;
    Vector3 dir;
    int tpIndex = 0;

    public float hp = 100f;
    public float initHp = 100f;
    public float maxHp = 100f;
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
        Dictionary<GameObject, string> turnPoints = new Dictionary<GameObject, string>();
        enemyTr = GetComponent<Transform>();
        GameObject[] tps = GameObject.FindGameObjectsWithTag("TurnPoints");
        for (int i = 0; i < tps.Length; i++)
        {
            turnPoints.Add(tps[i], tps[i].name);
        }
        turnPoints = turnPoints.OrderBy(tp => tp.Value).ToDictionary(tp => tp.Key, tp => tp.Value);
        turns = turnPoints.Keys.ToList();
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            if (tpIndex < turns.Count)
            {
                dir = (turns[tpIndex].transform.position - enemyTr.position);
                enemyTr.Translate(dir.normalized * moveSpeed * Time.deltaTime);
                if (dir.magnitude < Vector3.forward.magnitude * 0.1)
                {
                    enemyTr.position = turns[tpIndex].transform.position;
                    dir = (turns[tpIndex].transform.position - enemyTr.position);
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
            gm.Life--;
            //Debug.Log(gm.Life);
            ++gm.RemovedEnemyCnt;
            if (gm.SelectedObject == gameObject)
            {
                gm.SelectedObject = null;
            }
            StopCoroutine(MoveCoroutine());
            InitEnemy();
        }
    }

    public void EnemyDead()
    {
        if (gm.SelectedObject == gameObject)
        {
            gm.SelectedObject = null;
        }
        StopCoroutine(MoveCoroutine());
        ++gm.RemovedEnemyCnt;
        //Debug.Log($"removed: {gm.RemovedEnemyCnt}");
        ++gm.TotalKills;
        //Debug.Log(gm.TotalKills);
        InitEnemy();
    }

    public void InitEnemy()
    {
        if(spawn==null)
            spawn = GameObject.FindGameObjectWithTag("Spawn");
        // 렌더러 끄기
        gameObject.name = $"Round {GameManager.Instance.Rounds}";
        transform.position = spawn.transform.position;
        tpIndex = 0;
        initHp += 50 * ((GameManager.Instance.Rounds - 1)/ 5);
        maxHp = initHp * GameManager.Instance.Rounds * GameManager.Instance.Ratio;
        hp = maxHp;
        gameObject.SetActive(false);
        // 라운드에 따른 hp 상승 및 초기화      
    }
}
