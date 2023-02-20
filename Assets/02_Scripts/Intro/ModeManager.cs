using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public string modeName;
    public int gold;
    public int life;
    public int ratio;
    GameManager gm;
    void Start()
    {
        gm = GameManager.Instance;
    }

    public void SetDifficulty()
    {
        gm.DifficultyName = modeName;
        gm.Gold = gold;
        gm.Life = life;
        gm.Ratio = ratio;
    }
}
