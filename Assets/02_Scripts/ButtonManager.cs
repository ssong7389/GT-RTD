using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
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
    Image mainBtnIcon;
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
    public Button towerListBackBtn;

    public GameObject gameOverPanel;
    Button overIntroBtn;
    Button overRetryBtn;

    SpriteAtlas atlas;
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


        towerListBackBtn.onClick.AddListener(() => OnTowerListBackBtbClicked());

        atlas = Resources.Load<SpriteAtlas>("characters/characters_atlas");
        mainBtnIcon = mainBtn.transform.Find("Icon").GetComponent<Image>();

        overIntroBtn = gameOverPanel.transform.Find("Intro").GetComponent<Button>();
        overIntroBtn.onClick.AddListener(() => OnSettingsIntroClicked());

        overRetryBtn = gameOverPanel.transform.Find("Retry").GetComponent<Button>();
        overRetryBtn.onClick.AddListener(() => OnRetryBtnClicked());

        gameOverPanel.SetActive(false);
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

    public void SetMainBtnIcon(GameManager.Selected selected)
    {        
        if(selected == GameManager.Selected.TOWER_AREA)
        {
            mainBtnIcon.gameObject.SetActive(true);
            mainBtnIcon.sprite = atlas.GetSprite("icon_build");
        }
        else if(selected == GameManager.Selected.TOWER)
        {
            mainBtnIcon.gameObject.SetActive(true);
            mainBtnIcon.sprite = atlas.GetSprite("icon_merge");
        }
        else
        {
            mainBtnIcon.gameObject.SetActive(false);
        }
    }

    private void OnRetryBtnClicked()
    {
        //Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
