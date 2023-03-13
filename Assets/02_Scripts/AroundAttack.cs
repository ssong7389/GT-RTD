using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class AroundAttack : MonoBehaviour
{
    GameObject effect;
    Animator anim;
    Tower tower;
    void Start()
    {
        tower = GetComponent<Tower>();
        effect = transform.Find("Effect").gameObject;
        anim = effect.GetComponent<Animator>();
        effect.SetActive(false);
    }

    public void PlayAroundAttack()
    {
        effect.transform.position = transform.position;
        effect.SetActive(true);
        anim.Play("attack");
    }
}
