using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class StatusController : MonoBehaviour
{
    GameManager gm;

    GameObject statName;
    GameObject statHp;
    GameObject statDmg;

    Text nameText;
    Slider hpSlider;
    Text dmgText;

    Image portrait;
    Image towerIcon;
    SpriteAtlas uiAtlas;
    SpriteAtlas characterAtals;
    void Start()
    {
        gm = GameManager.Instance;
        statName = transform.Find("StatusName").gameObject;
        nameText = statName.GetComponent<Text>();
        statHp = transform.Find("StatusHp").gameObject;
        hpSlider = statHp.GetComponent<Slider>();
        statDmg = transform.Find("StatusDmg").gameObject;
        dmgText = statDmg.GetComponent<Text>();
        characterAtals = Resources.Load<SpriteAtlas>("characters/characters_atlas");
        portrait = transform.Find("Portrait").GetComponent<Image>();
        uiAtlas = Resources.Load<SpriteAtlas>("ui_atlas");
        towerIcon = transform.Find("TowerIcon").GetComponent<Image>();
    }

    void ChangeSprite(string portraitName)
    {
        portrait.sprite = characterAtals.GetSprite(portraitName);
        portrait.SetNativeSize();
        float width = portrait.rectTransform.rect.width;
        float height = portrait.rectTransform.rect.height;
        portrait.rectTransform.sizeDelta = new Vector2(width * 1.5f, height * 1.5f);
    }

    public void DisplayTower()
    {
        //if (gm.SelectedObject == null)
        //{
        //    Debug.Log("Null");
        //}
        nameText.text = GameManager.Instance.SelectedObject.name;
        statName.SetActive(true);
        statHp.SetActive(false);
        statDmg.SetActive(true);
        towerIcon.gameObject.SetActive(true);
        Tower selectedTower = gm.SelectedObject.GetComponent<Tower>();
        TowerManager.Type type = selectedTower.Type;
        towerIcon.sprite = uiAtlas.GetSprite($"upgrade_{type}");
        ChangeSprite(selectedTower.portraitName);
        if (TowerManager.Instance.GetUpgrade(selectedTower.Type) == 0)
            dmgText.text = $" {selectedTower.dmg}";
        else
        {
            dmgText.text = $" {selectedTower.dmg}(+{selectedTower.increment} x {TowerManager.Instance.GetUpgrade(selectedTower.Type)})";
        }
    }
    public void DisplayEnemy()
    {
        nameText.text = gm.SelectedObject.name;
        statName.SetActive(true);
        statDmg.SetActive(false);
        statHp.SetActive(true);
        towerIcon.gameObject.SetActive(false);
        EnemyController enemyController = gm.SelectedObject.GetComponent<EnemyController>();
        ChangeSprite(enemyController.enemyAnim.GetPortriatName());
        hpSlider.value = enemyController.hp / enemyController.maxHp;
    }
}
