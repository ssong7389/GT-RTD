using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    private static ButtonManager _instance;

    public static ButtonManager Instance
    {
        get
        {
            return _instance;
        }
    }
    public Button mainBtn;
    public Button MainBtn
    {
        get { return mainBtn; }
    }
    public Button sellBtn;
    public Button exchangeBtn;
    public GameObject crystalPopup;

    GameManager gm;
    TowerManager tm;

    public Button settingsBtn;
    public GameObject settingsPopup;
    Transform settingsPanel;
    Button settingsConfirm;
    Button settingsIntro;

    public Button[] towerListBtns;
    Button clickedBtn;
    GameObject clickedList;
    public Button upperTowerListBtn;
    public GameObject towerListPopup;
    Button towerListBackBtn;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        sellBtn.onClick.AddListener(() => OnSellBtnClicked(gm.SelectedObject));
        sellBtn.gameObject.SetActive(false);

        exchangeBtn.onClick.AddListener(() => OnExchangeBtnClicked());


        settingsBtn.onClick.AddListener(() => OnSettingsBtnClicked());
        settingsPanel = settingsPopup.transform.GetChild(0);

        settingsConfirm = settingsPanel.transform.Find("Confirm").GetComponent<Button>();
        settingsConfirm.onClick.AddListener(() => OnSettingsConfirmClicked());

        settingsIntro = settingsPanel.Find("Intro").GetComponent<Button>();
        settingsIntro.onClick.AddListener(() => OnSettingsIntroClicked());

        upperTowerListBtn.onClick.AddListener(() => OnUpperTowerListBtnClicked());

        towerListBackBtn = towerListPopup.transform.Find("Back").GetComponent<Button>();
        towerListBackBtn.onClick.AddListener(() => OnTowerListBackBtbClicked());
    }

    void Start()
    {
        gm = GameManager.Instance;
        tm = TowerManager.Instance;
        foreach (Button towerListBtn in towerListBtns)
        {
            towerListBtn.onClick.AddListener(() => OnTowerListBtnClicked(towerListBtn));
        }
        clickedBtn = towerListBtns[0];
        towerListBtns[0].interactable = false;
        clickedList = towerListBtns[0].transform.Find($"TowerList_{towerListBtns[0].name}").gameObject;
    
    }
    void Update()
    {
        // test
        if(gm.selected == GameManager.Selected.TOWER_AREA)
        {
            mainBtn.GetComponentInChildren<Text>().text = "Build";
        }
        else if(gm.selected == GameManager.Selected.TOWER)
        {
            mainBtn.GetComponentInChildren<Text>().text = "Merge";
            sellBtn.gameObject.SetActive(true);
        }
        else if(gm.selected == GameManager.Selected.NONE)
        {
            mainBtn.GetComponentInChildren<Text>().text = "";
            sellBtn.gameObject.SetActive(false);
        }
        else if(gm.selected == GameManager.Selected.ENEMY)
        {
            mainBtn.GetComponentInChildren<Text>().text = "";
            sellBtn.gameObject.SetActive(false);
        }
    }

    public void OnBuildBtnClicked()
    {
        tm.BuildTower(GameManager.Instance.SelectedObject.transform);
    }
    private void OnSellBtnClicked(GameObject towerToSell)
    {
        if(towerToSell.layer == 7)
        {
            tm.SellTower(towerToSell);
        }

    }
    public void OnMergeBtnClicked(GameObject towerToMerge)
    { 
        tm.MergeTower(towerToMerge);
    }

    private void OnExchangeBtnClicked()
    {
        if (gm.Gold < 100)
            return;

        gm.Gold -= 100;
        gm.Gem += Random.Range(gm.MinRandomGem, gm.MaxRandomGem + 1);
    }

    private void OnSettingsBtnClicked()
    {
        Time.timeScale = 0;
        settingsPopup.SetActive(true);
    }

    private void OnSettingsConfirmClicked()
    {
        Time.timeScale = 1f;
        settingsPopup.SetActive(false);
    }
    private void OnSettingsIntroClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    private void OnUpperTowerListBtnClicked()
    {
        Time.timeScale = 0;
        towerListPopup.SetActive(true);
    }

    private void OnTowerListBtnClicked(Button self)
    {
        clickedBtn.interactable = true;
        clickedList.SetActive(false);

        self.interactable = false;
        clickedBtn = self;
        clickedList = self.transform.Find($"TowerList_{self.name}").gameObject;
        clickedList.SetActive(true);
    }

    private void OnTowerListBackBtbClicked()
    {
        Time.timeScale = 1f;
        towerListPopup.SetActive(false);
    }
}
