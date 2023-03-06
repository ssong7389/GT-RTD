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
    public int end;
    GameManager gm;

    Text modeNameText;
    Text modeInfoText;
    
    void Start()
    {
        gm = GameManager.Instance;
    }


    public void SetText()
    {
        modeNameText = transform.Find("ModeNameText").GetComponent<Text>();
        modeInfoText = transform.Find("ModeInfoText").GetComponent<Text>();
        modeNameText.text = modeName;
        modeInfoText.text = $"Life: {life}\nGold:{gold}\nHp:{ratio * 100}";
        if (end == 0)
            modeInfoText.text += $"\nRounds: Infinity";
        else
            modeInfoText.text += $"\nRounds: {end}";
    }
    public void OnModeBtnClicked()
    {
        GameManager.Instance.InitGameData(modeName, gold, life, ratio, end);
        SceneManager.LoadScene(1);
    }
}
