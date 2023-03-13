using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        if (tower.State == Tower.States.attack)
        {


        }
        else
        {
            effect.SetActive(false);
        }
    }

    public void PlayRangeAttack()
    {
        effect.transform.position = tower.Target.transform.position;
        effect.SetActive(true);
        anim.Play("attack");
    }
}
