using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IntroManager : MonoBehaviour
{
    List<Dictionary<string, object>> modes;
    string modesPath = "Modes";
    public GameObject modeContent;
    public GameObject modePrefab;
    private void Awake()
    {
        modes = CSVReader.Read(modesPath);
        //modeContent = GameObject.FindGameObjectWithTag("ModeContent");
    }
    private void Start()
    {

        for (int i = 0; i < modes.Count; i++)
        {
            GameObject mode = Instantiate(modePrefab);
            ModeManager modeManager = mode.GetComponent<ModeManager>();
            modeManager.modeName = modes[i]["ModeName"].ToString();
            modeManager.life = (int)modes[i]["Life"];
            modeManager.gold = (int)modes[i]["Gold"];
            modeManager.ratio = (int)modes[i]["Ratio"];
            modeManager.end = (int)modes[i]["End"];
            modeManager.SetText();
            mode.GetComponent<Button>().onClick.AddListener(() => modeManager.OnModeBtnClicked());
            mode.transform.SetParent(modeContent.transform, false);
        }

    }


}
