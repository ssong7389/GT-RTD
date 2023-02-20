using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBgManager : MonoBehaviour
{
    GameObject difficulty;
    void Start()
    {
        difficulty = GameObject.Find("Modes");
        difficulty.SetActive(false);
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            difficulty.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
