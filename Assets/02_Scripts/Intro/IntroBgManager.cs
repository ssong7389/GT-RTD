using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroBgManager : MonoBehaviour
{
    GameObject modes;
    void Start()
    {
        modes = GameObject.Find("Modes");
        modes.SetActive(false);
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            modes.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
