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
    GameObject[] enemies;

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
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        //Debug.Log(gm.IsRoundClear);
            yield return new WaitForSeconds(gm.PreTime);
            gm.IsRoundClear = false;
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(true);
            enemies[i].GetComponent<EnemyController>().MoveToArrival();
            yield return new WaitForSeconds(spawnRate);
        }
        yield return new WaitUntil(() => gm.IsRoundClear);
        StartCoroutine(StartRound());
        //gm.Rounds++;
    }

}
