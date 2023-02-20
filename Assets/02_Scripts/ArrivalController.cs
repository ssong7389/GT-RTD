using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrivalController : MonoBehaviour
{
    GameManager gm;
    void Start()
    {
        gm = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ENEMY"))
        {
            gm.Life--;
            //Debug.Log(gm.Life);
            ++gm.RemovedEnemyCnt;
        }
    }
}
