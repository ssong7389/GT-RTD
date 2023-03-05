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
            if (direction == Dir.left)
            {
                transform.localScale = new Vector3(-originScaleXY, originScaleXY, 1f);
            }
            else
            {
                transform.localScale = new Vector3(originScaleXY, originScaleXY, 1f);
            }
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
    private void Awake()
    {
        originScaleXY = transform.localScale.x;
        skel = GetComponent<SkeletonAnimation>();
        meshRenderer = GetComponent<MeshRenderer>();
        List<Dictionary<string, object>> data = CSVReader.Read("enemy/Enemy");
        enemyNames = new string[data.Count];
        for (int i = 0; i < data.Count; i++)
        {
            //Debug.Log(data[i]["Name"].ToString());
            enemyNames[i] = data[i]["Name"].ToString();
        }
    }

    public void SetSkeleton(int round)
    {
        int index = round - 1;
        if (index > 50)
            index %= 50;
        //Debug.Log("index : " + index);
        string path = $"enemy/{enemyNames[index]}/{enemyNames[index]}";
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
                break;
            case Dir.left:
                strDir = "side";
                break;
            default:
                strDir = dir.ToString();
                break;
        }
        newSkeletonData = (SkeletonDataAsset)Resources.Load($"{path}_{strDir}_SkeletonData");
        //skel.skeletonDataAsset = null;
        //skel.skeletonDataAsset.Clear();
        if (skel.SkeletonDataAsset != newSkeletonData)
        {
            skel.skeletonDataAsset = newSkeletonData;
        }
        skel.Initialize(true);
        skel.AnimationName = $"{State}_{strDir}";
    }
}
