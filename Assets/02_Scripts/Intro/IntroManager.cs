using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;


public class IntroManager : MonoBehaviour
{
    List<Dictionary<string, object>> modes;
    string modesPath = "Modes";
    public GameObject modeContent;
    public GameObject modePrefab;

    public Button how;
    public Button exit;
    public GameObject howToPlay;

    SpriteAtlas atlas;
    private void Awake()
    {
        modes = CSVReader.Read(modesPath);
        atlas = Resources.Load<SpriteAtlas>("items/items_atlas");
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
            modeManager.spriteName = modes[i]["SpriteName"].ToString();
            modeManager.SetText();

            if (modeManager.modeIcon == null)
            {
                Debug.Log(modeManager.spriteName);
            }
            modeManager.modeIcon.sprite = atlas.GetSprite(modeManager.spriteName);
            mode.GetComponent<Button>().onClick.AddListener(() => modeManager.OnModeBtnClicked());
            mode.transform.SetParent(modeContent.transform, false);
        }
        how.onClick.AddListener(()=>OnHowToPlay());
        exit.onClick.AddListener(() => OnExitBtn());
    }
    private void OnHowToPlay()
    {
        howToPlay.SetActive(true);
    }

    private void OnExitBtn()
    {
        Application.Quit();
    }

}
