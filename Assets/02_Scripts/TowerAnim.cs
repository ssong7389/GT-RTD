using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class TowerAnim : MonoBehaviour
{
    SkeletonAnimation skel;
    public string animationName;
    string path;
    enum TowerState
    {
        IDLE, ATTACK, VICTORY, LOSE, CLEAR
    }
    void Start()
    {
        skel = GetComponent<SkeletonAnimation>();
        string dir = gameObject.name.Substring(gameObject.name.LastIndexOf('_'));
        skel.AnimationName = "rifle_shoot2" + dir;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(skel.AnimationState);
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            skel.skeletonDataAsset = (SkeletonDataAsset)Resources.Load("Anims/future_knight/future_knight_side_SkeletonData");
            skel.Initialize(true);
            skel.AnimationName = "rifle_shoot2_side";
        }
    }
}
