using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameInfoManager : MonoBehaviour
{
    Text modeNameText;
    public Text roundText;
    public Text lifeText;
    public Text killsText;
    Transform infoTr;
    void Awake()
    {
        infoTr = GetComponent<Transform>();
        modeNameText = infoTr.Find("ModeName").GetComponent<Text>();
        roundText = infoTr.Find("Rounds").GetComponent<Text>();
        lifeText = infoTr.Find("Life").GetComponent<Text>();
        killsText = infoTr.Find("Kills").GetComponent<Text>();

        modeNameText.text = $"{GameManager.Instance.Mode} ({GameManager.Instance.Ratio * 100}%)";
        if (GameManager.Instance.End != 0)
            modeNameText.text += $" {GameManager.Instance.End} Rounds";
    }

}
