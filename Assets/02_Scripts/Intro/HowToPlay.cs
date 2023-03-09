using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    public GameObject[] descriptions;
    Button next;
    int index = 0;
    void Start()
    {
        next = transform.Find("Next").GetComponent<Button>();
        next.onClick.AddListener(() => OnNext());
    }

    private void OnNext()
    {
        if (index == descriptions.Length -1)
        {
            descriptions[index].SetActive(false);
            index = 0;
            descriptions[index].SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            descriptions[index].SetActive(false);
            descriptions[++index].SetActive(true);
        }
    }
}
