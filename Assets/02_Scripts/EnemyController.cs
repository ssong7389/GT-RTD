using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Spine.Unity;

public class EnemyController : MonoBehaviour
{
    GameManager gm;

    Transform enemyTr;
    public float moveSpeed = 0.45f;

    GameObject spawn;
    GameObject arrival;
    
    List<GameObject> turns;
    Vector3 dir;
    [SerializeField]
    int tpIndex = 0;
    public float hp = 100f;
    public float initHp = 100f;
    public float maxHp = 100f;
    private bool deadPlaying = false;

    public EnemyAnimController enemyAnim;
    StatusController sc;
    string path;
    void Awake()
    {
        gm = GameManager.Instance;

        spawn = GameObject.FindGameObjectWithTag("Spawn");
        arrival = GameObject.FindGameObjectWithTag("Arrival");
        enemyAnim = GetComponent<EnemyAnimController>();

        //MoveToArrival();
    }
    private void Update()
    {
        if (hp <= 0 && !deadPlaying)
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
        //enemyAnim.Direction = EnemyAnimController.Dir.front;
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        while (true)
        {
            if (GameManager.Instance.Life <= 0)
            {
                break;
            }
            if(hp <= 0)
            {
                break;
            }
            if (tpIndex < turns.Count)
            {

                dir = (turns[tpIndex].transform.position - enemyTr.position);
                enemyTr.Translate(dir.normalized * moveSpeed * Time.deltaTime);
                if (dir.magnitude < Vector3.forward.magnitude * 0.1)
                {
                    enemyTr.position = turns[tpIndex].transform.position;
                    dir = (turns[tpIndex].transform.position - enemyTr.position);

                    int dirInt = (int)enemyAnim.Direction;
                    enemyAnim.Direction = (EnemyAnimController.Dir)(++dirInt % 4);
                    enemyAnim.SetSkeleton(GameManager.Instance.Rounds);
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
            if (gameObject.CompareTag("BOSS"))
            {
                gm.Life -= 10;
                gm.RemovedEnemyCnt += 20;
                if (gm.SelectedObject == gameObject)
                {
                    gm.SelectedObject = null;
                }
                AudioManager.Instance.StopBgm();
            }
            else
            {
                gm.Life--;
                //Debug.Log(gm.Life);
                ++gm.RemovedEnemyCnt;
                if (gm.SelectedObject == gameObject)
                {
                    gm.SelectedObject = null;
                }
                StopCoroutine(MoveCoroutine());
            }
            InitEnemy();
        }
    }

    public void EnemyDead()
    {
        deadPlaying = true;
        if (gm.SelectedObject == gameObject)
        {
            gm.SelectedObject = null;
        }
        StopCoroutine(MoveCoroutine());
        GetComponent<BoxCollider>().enabled = false;
        if (CompareTag("BOSS"))
        {
            ++gm.Crystals;
            // 보스 처치 연출 등
            AudioManager.Instance.StopBgm();
            GameManager.Instance.IsRoundClear = true;
        }
        else
        {
            StartCoroutine(DeadCoroutine());

            //Debug.Log($"removed: {gm.RemovedEnemyCnt}");
            ++gm.TotalKills;
            //Debug.Log(gm.TotalKills);
        }
    }
    IEnumerator DeadCoroutine()
    {
        //Debug.Log("dead");
        enemyAnim.DeadAnimation();
        yield return new WaitForSeconds(2.367f);
        ++gm.RemovedEnemyCnt;
        InitEnemy();

    }
    public void GenerateEnemy(float initHp, float maxHp)
    {
        enemyAnim = GetComponent<EnemyAnimController>();
        enemyAnim.Direction = EnemyAnimController.Dir.front;
        enemyAnim.SetSkeleton(GameManager.Instance.Rounds);
        GetComponent<BoxCollider>().enabled = false;
        gameObject.SetActive(false);
        if (spawn == null)
            spawn = GameObject.FindGameObjectWithTag("Spawn");
        transform.position = spawn.transform.position;
        tpIndex = 0;
        this.initHp = initHp;
        this.maxHp = maxHp;
        hp = maxHp;
        gameObject.name = $"Round {GameManager.Instance.Rounds}";
    }
    public void InitEnemy()
    {
        int nextRound = GameManager.Instance.Rounds + 1;
        deadPlaying = false;
        gameObject.SetActive(false);
        GetComponent<BoxCollider>().enabled = false;

        if (GameManager.Instance.Life <= 0)
            return;
        if (spawn == null)
            spawn = GameObject.FindGameObjectWithTag("Spawn");

        transform.position = spawn.transform.position;
        tpIndex = 0;
        enemyAnim.Direction = EnemyAnimController.Dir.front;
        initHp += 50 * ((nextRound - 1) / 5);
        if (gameObject.CompareTag("ENEMY"))
        {
            gameObject.name = $"Round {nextRound}";
            maxHp = initHp * nextRound * GameManager.Instance.Ratio;
            hp = maxHp;
            if (nextRound % 10 != 0)
            {
                enemyAnim.SetSkeleton(nextRound);
            }
        }
        if (gameObject.CompareTag("BOSS"))
        {
            int round = GameManager.Instance.Rounds;
            gameObject.name = $"Round {round} Boss";
            maxHp = initHp * round * GameManager.Instance.Ratio;
            maxHp *= GameManager.Instance.unitsPerRound;
            hp = maxHp;
            if(nextRound % 10 == 0)
            {
                enemyAnim.Direction = EnemyAnimController.Dir.front;
            }
        }     
    }

    public void EnemyGameOver()
    {
        StopCoroutine(MoveCoroutine());
        if (gameObject.activeSelf)
        {
            enemyAnim.GameOverAnimation();
        }

    }
}
