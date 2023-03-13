using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    SpriteRenderer sprenderer;

    void Awake()
    {
        sprenderer = GetComponent<SpriteRenderer>();
    }

    public void SetBackground()
    {
        Sprite sprite = Resources.Load<Sprite>($"bg/bg_0{GameManager.Instance.Rounds / 10 + 1}");
        sprenderer.sprite = sprite;
    }
}
