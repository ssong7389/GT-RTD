using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    void Start()
    {
        gm = GameManager.Instance;
        statName = transform.Find("StatusName").gameObject;
        nameText = statName.GetComponent<Text>();
        statHp = transform.Find("StatusHp").gameObject;
        hpSlider = statHp.GetComponent<Slider>();
        statDmg = transform.Find("StatusDmg").gameObject;
        dmgText = statDmg.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
       switch (gm.selected)
        {
            case GameManager.Selected.TOWER:
                nameText.text = gm.SelectedObject.name;
                statName.SetActive(true);
                statHp.SetActive(false);
                statDmg.SetActive(true);
                Tower selectedTower = gm.SelectedObject.GetComponent<Tower>();
                if (TowerManager.Instance.GetUpgrade(selectedTower.Type) == 0)
                    dmgText.text = $"공격력: {selectedTower.dmg}";
                else
                {
                    dmgText.text = $"공격력: {selectedTower.dmg}(+{selectedTower.increment} x {TowerManager.Instance.GetUpgrade(selectedTower.Type)})";
                }
                break;
            case GameManager.Selected.ENEMY:
                nameText.text = gm.SelectedObject.name;
                statName.SetActive(true);
                statDmg.SetActive(false);
                statHp.SetActive(true);
                EnemyController enemyController = gm.SelectedObject.GetComponent<EnemyController>();
                hpSlider.value = enemyController.hp / enemyController.maxHp;
                break;
            default:
                statName.SetActive(false);
                statHp.SetActive(false);
                statDmg.SetActive(false);
                break;
        }
    }
}
