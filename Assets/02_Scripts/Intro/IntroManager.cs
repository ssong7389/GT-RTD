using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    List<Dictionary<string, object>> modes;
    string modesPath = "Modes";
    GameObject modeContent;
    GameObject modePrefab;
    private void Start()
    {
        modes = CSVReader.Read(modesPath);
        modeContent = GameObject.FindGameObjectWithTag("ModeContent");
        for (int i = 0; i < modes.Count; i++)
        {
            GameObject mode = Instantiate(modePrefab);
            ModeManager modeManager = mode.GetComponent<ModeManager>();
            modeManager.modeName = modes[i]["ModeName"].ToString();
            modeManager.life = (int)modes[i]["Life"];
            modeManager.gold = (int)modes[i]["Gold"];
            modeManager.ratio = (int)modes[i]["Ratio"];
            mode.transform.parent = modeContent.transform;
        }

    }
    void OnDiffBtnClicked(ModeManager diffMgr)
    {
        diffMgr.SetDifficulty();
        SceneManager.LoadScene(1);
    }

}
