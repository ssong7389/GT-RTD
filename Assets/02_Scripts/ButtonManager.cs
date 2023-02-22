using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        sellBtn.onClick.AddListener(() => OnSellBtnClicked(gm.SelectedObject));
        sellBtn.gameObject.SetActive(false);

        exchangeBtn.onClick.AddListener(() => OnExchangeBtnClicked());
    }

    void Start()
    {
        gm = GameManager.Instance;
        tm = TowerManager.Instance;
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
}
