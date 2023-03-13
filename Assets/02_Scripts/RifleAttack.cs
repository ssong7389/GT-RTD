using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class RifleAttack : MonoBehaviour
{
    SkeletonAnimation skel;
    Animator anim;
    GameObject effect;
    Tower tower;
    Transform back, front, side;
    void Start()
    {
        //back = new Vector3(0, 0.9f, 0);
        //front = new Vector3(0, -0.9f, 0);
        //side = new Vector3(0.9f, 0, 0);
        back = transform.Find("muzzle_back");
        front = transform.Find("muzzle_front");
        side = transform.Find("muzzle_side");
        skel = GetComponent<SkeletonAnimation>();
        effect = transform.Find("Effect").gameObject;
        anim = effect.GetComponent<Animator>();
        tower = GetComponent<Tower>();
        //aim = skel.Skeleton.FindBone($"b.w1_axis");
        effect.SetActive(false);
    }

    public void PlayRifleAttack()
    {
        if(tower.Direction == Tower.Dir.front)
        {
            effect.transform.position = front.position;
            effect.transform.localRotation = Quaternion.Euler(0, 0, -90);
        }
        else if (tower.Direction == Tower.Dir.back)
        {
            effect.transform.position = back.position;
            effect.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            effect.transform.position = side.position;
            effect.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        effect.SetActive(true);
        anim.Play("attack");
    }
}
