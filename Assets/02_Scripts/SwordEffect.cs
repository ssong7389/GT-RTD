using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class SwordEffect : MonoBehaviour
{
    Bone axis;
    SkeletonAnimation skel;
    SkeletonAnimation effect;
    Tower tower;
    // Start is called before the first frame update
    void Start()
    {
        tower = GetComponent<Tower>();
        skel = GetComponent<SkeletonAnimation>();
        //axis = skel.Skeleton.FindBone($"[base]weapon1_{tower.Direction}");
        axis = skel.Skeleton.FindBone($"b.w1_axis");
        var bones = skel.Skeleton.Bones;
        //foreach (var bone in bones)
        //{
        //    Debug.Log(bone);
        //}
        effect = transform.Find("Effect").GetComponent<SkeletonAnimation>();


        effect.gameObject.SetActive(false);
        effect.loop = false;
        effect.AnimationName = "attack_1";
        effect.Initialize(true);

    }

    //void Update()
    //{
    //    if (tower.State == Tower.States.attack && tower.isAttack)
    //    {
           
    //    }
    //}
    public void PlayEffect()
    {
        //axis = skel.Skeleton.FindBone($"[base]weapon1_{tower.Direction}");
        axis.UpdateWorldTransform();
        effect.transform.position = axis.GetWorldPosition(transform);
        effect.transform.rotation = Quaternion.Euler(0, 0, tower.atkAngle);
        //Debug.Log(axis.GetLocalQuaternion().eulerAngles);
        //effect.transform.rotation = new Quaternion(0, 0, -tower.angle, axis.GetLocalQuaternion().w);
        //Debug.Log(effect.transform.rotation.z);

        effect.transform.localScale = new Vector3(-0.75f, -0.75f, 0.75f);
        
        effect.gameObject.SetActive(true);
        effect.AnimationName = "attack_1";
        effect.Initialize(true);
    }
}
