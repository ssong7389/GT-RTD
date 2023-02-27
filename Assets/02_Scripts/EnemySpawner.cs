using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    GameManager gm;

    public int enemyCnt = 20;
    Transform spawnPoint;
    public float spawnRate = 0.2f;

    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    GameObject[] enemies;
    GameObject boss;
    EnemyController bossController;
    void Start()
    {
        gm = GameManager.Instance;
        spawnPoint = GetComponent<Transform>();
        enemies = new GameObject[enemyCnt];
        for (int i = 0; i < enemyCnt; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);

            enemies[i] = enemy;
            EnemyController enemyCtrl = enemies[i].GetComponent<EnemyController>();
            enemyCtrl.maxHp = 100f;
            enemyCtrl.initHp = 100f;
            enemyCtrl.InitEnemy();
        }
        boss = Instantiate(bossPrefab);
        bossController = boss.GetComponent<EnemyController>();
        bossController.maxHp = 100f;
        bossController.initHp = 100f;
        boss.SetActive(false);
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        //Debug.Log(gm.IsRoundClear);
        yield return new WaitForSeconds(gm.PreTime);
        gm.IsRoundClear = false;

        if (GameManager.Instance.Rounds % 10 == 0)
        {
            bossController.InitEnemy();
            boss.SetActive(true);
            boss.GetComponent<BoxCollider>().enabled = true;
            bossController.MoveToArrival();
        }
        else
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                //enemies[i].transform.position = spawnPoint.position;
                enemies[i].SetActive(true);
                enemies[i].GetComponent<BoxCollider>().enabled = true;
                enemies[i].GetComponent<EnemyController>().MoveToArrival();
                yield return new WaitForSeconds(spawnRate);
            }
        }
        yield return new WaitUntil(() => gm.IsRoundClear);
        if (GameManager.Instance.End == 0)
        {
            StartCoroutine(StartRound());
        }
        else
        {            
            if (GameManager.Instance.Rounds > GameManager.Instance.End)
            {
                // ���� Ŭ����
                yield return null;
            }
            else
            {
                StartCoroutine(StartRound());
            }
        }
    }

    IEnumerator StartBossRound()
    {
        yield return new WaitForSeconds(gm.PreTime);
        gm.IsRoundClear = false;



    }
}
