using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EnemyAnimController : MonoBehaviour
{
    SkeletonAnimation skel;
    MeshRenderer meshRenderer;
    float originScaleXY;
    public enum Dir
    {
        front, right, back, left
    }
    private Dir direction;
    public Dir Direction 
    {
        get { return direction; }
        set
        {
            direction = value;
        }
    }
    public enum States
    {
        run, dead
    }
    private States state;
    public States State
    {
        get { return state; }
        set
        {
            state = value;
        }
    }
    // front side back side front side back side front side back
    string[] enemyNames;
    string[] enemyPortraits;
    public string path;
    private void Awake()
    {
        originScaleXY = transform.localScale.x;
        skel = GetComponent<SkeletonAnimation>();
        meshRenderer = GetComponent<MeshRenderer>();
        List<Dictionary<string, object>> data = CSVReader.Read("enemy/Enemy");
        enemyNames = new string[data.Count];
        enemyPortraits = new string[data.Count];
        for (int i = 0; i < data.Count; i++)
        {
            enemyNames[i] = data[i]["Name"].ToString();
            enemyPortraits[i] = data[i]["Portrait"].ToString();
        }
        path = $"enemy/{enemyNames[0]}/{enemyNames[0]}";
    }

    public void SetSkeleton(int round)
    {
        int index = round - 1;
        if (index >= 50)
            index %= 50;
        //Debug.Log("index : " + index);
        path = $"enemy/{enemyNames[index]}/{enemyNames[index]}";
        //Debug.Log(path);
        meshRenderer = GetComponent<MeshRenderer>();
        //Debug.Log($"{path}_Material");
        meshRenderer.sharedMaterial = (Material)Resources.Load($"{path}_Material");
        meshRenderer.sortingOrder = -1;
        //Debug.Log($"{path}_{direction}_SkeletonData");
        SetSkeletonDataDirection(path, Direction);
    }
    public void SetSkeletonDataDirection(string path, Dir dir)
    {
        SkeletonDataAsset newSkeletonData;
        string strDir;
        switch (dir)
        {
            case Dir.right:
                strDir = "side";
                transform.localScale = new Vector3(originScaleXY, originScaleXY, 1f);
                break;
            case Dir.left:
                strDir = "side";
                transform.localScale = new Vector3(-originScaleXY, originScaleXY, 1f);
                break;
            default:
                strDir = dir.ToString();
                break;
        }
        newSkeletonData = (SkeletonDataAsset)Resources.Load($"{path}_{strDir}_SkeletonData");

        if (skel.SkeletonDataAsset != newSkeletonData)
        {
            skel.skeletonDataAsset = newSkeletonData;
        }
        skel.loop = true;
        skel.Initialize(true);
        if (gameObject.CompareTag("ENEMY"))
        {
            skel.AnimationName = $"{State}_{strDir}";
        }
        else
        {
            skel.AnimationName = $"walk_{strDir}";
        }
    }
    public string GetPortriatName()
    {
        return enemyPortraits[GameManager.Instance.Rounds - 1];
    }
    public void GameOverAnimation()
    {
        skel.skeletonDataAsset = Resources.Load<SkeletonDataAsset>($"{path}_side_SkeletonData");
        skel.Initialize(true);
        skel.AnimationName = $"dance_side";
    }

    public void DeadAnimation()
    {
        skel.skeletonDataAsset = Resources.Load<SkeletonDataAsset>($"{path}_side_SkeletonData");

        skel.AnimationName = $"dead_side";
        skel.Initialize(true);
    }
    public void BossDeadAnimation()
    {
        skel.skeletonDataAsset = Resources.Load<SkeletonDataAsset>($"{path}_side_SkeletonData");
        if (GameManager.Instance.Rounds % 50 == 0)
        {
            skel.loop = false;
            skel.AnimationName = $"monster_damaged_side";
            skel.Initialize(true);
        }
        else
        {
            skel.loop = false;
            skel.AnimationName = $"damaged_side";
            skel.Initialize(true);
        }
    }
}
