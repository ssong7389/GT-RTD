using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModeManager : MonoBehaviour
{
    public string modeName;
    public int gold;
    public int life;
    public int ratio;
    GameManager gm;

    Text modeNameText;
    Text modeInfoText;
    
    void Start()
    {
        gm = GameManager.Instance;
        //modeNameText = transform.Find("ModeNameText").GetComponent<Text>();
        //modeInfoText = transform.Find("ModeInfoText").GetComponent<Text>();
    }

    public void SetDifficulty()
    {
        gm.DifficultyName = modeName;
        gm.Gold = gold;
        gm.Life = life;
        gm.Ratio = ratio;
    }
    public void SetText()
    {
        modeNameText = transform.Find("ModeNameText").GetComponent<Text>();
        modeInfoText = transform.Find("ModeInfoText").GetComponent<Text>();
        modeNameText.text = modeName;
        modeInfoText.text = $"Life: {life}\nGold:{gold}\nHp:{ratio * 100}%";
    }
    public void OnModeBtnClicked()
    {
        SetDifficulty();
        SceneManager.LoadScene(1);
    }
}
