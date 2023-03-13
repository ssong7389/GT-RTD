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
    EnemyAnimController bossAnimCtrl;
    public GameObject background;
    BackgroundController bg;
    void Start()
    {
        gm = GameManager.Instance;
        bg = background.GetComponent<BackgroundController>();
        spawnPoint = GetComponent<Transform>();
        enemies = new GameObject[enemyCnt];
        for (int i = 0; i < enemyCnt; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);

            enemies[i] = enemy;
            EnemyController enemyCtrl = enemies[i].GetComponent<EnemyController>();
            enemyCtrl.GenerateEnemy(100f, 100f*GameManager.Instance.Ratio);
        }
        boss = Instantiate(bossPrefab);
        bossController = boss.GetComponent<EnemyController>();
        bossController.maxHp = 100f;
        bossController.initHp = 100f;
        bossAnimCtrl = boss.GetComponent<EnemyAnimController>();
        //boss.GetComponent<BoxCollider>().enabled = false;
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
            AudioManager.Instance.StopBgm();
            AudioManager.Instance.PlayBgm("bgm_boss");
            bossAnimCtrl.Direction = EnemyAnimController.Dir.front;
            bossAnimCtrl.SetSkeleton(GameManager.Instance.Rounds);
            bossController.InitEnemy();
            boss.SetActive(true);
            boss.GetComponent<BoxCollider>().enabled = true;
            bossController.MoveToArrival();

            foreach (var enemy in enemies)
            {
                //Debug.Log(i++);
                enemy.GetComponent<EnemyController>().InitEnemy();
            }
        }
        else
        {
            if (!AudioManager.Instance.isPlaying)
            {
                AudioManager.Instance.PlayBgm($"bgm_0{GameManager.Instance.Rounds / 10 + 1}");
                bg.SetBackground();
            }
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].SetActive(true);
                enemies[i].GetComponent<BoxCollider>().enabled = true;
                enemies[i].GetComponent<EnemyController>().MoveToArrival();
                //enemies[i].GetComponent<EnemyAnimController>().Direction = EnemyAnimController.Dir.front;
                yield return new WaitForSeconds(spawnRate);
            }
        }

        yield return new WaitUntil(() => GameManager.Instance.IsRoundClear == true);
        GameManager.Instance.Rounds++;
        if (GameManager.Instance.End == 0)
        {
            StartCoroutine(StartRound());
        }
        else
        {            
            if (GameManager.Instance.Rounds > GameManager.Instance.End)
            {
                // 게임 클리어
                yield return null;
            }
            else
            {
                StartCoroutine(StartRound());
            }
        }
    }
}
