using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    Transform tr;
    void Start()
    {
        tr = transform;
        int typeLength = Enum.GetValues(typeof(TowerManager.Type)).Length;
        Button[] upgradeBtns = new Button[typeLength];
        for (int i = 0; i < typeLength; i++)
        {
            Button upgrade = tr.Find(((TowerManager.Type)i).ToString()).GetComponent<Button>();
            upgrade.onClick.AddListener(() => OnUpgradeBtnClicked(upgrade));
        }
    }
    public void OnUpgradeBtnClicked(Button self)
    {
        //Debug.Log("type: " + self.name);
        TowerManager.Type type = (TowerManager.Type)(Enum.Parse(typeof(TowerManager.Type), self.name));
        int cost = GameManager.Instance.UpgradeBaseGem + TowerManager.Instance.GetUpgrade(type);
        if (GameManager.Instance.Gem < cost)
        {
            return;
        }

        GameManager.Instance.Gem -= cost;
        TowerManager.Instance.Upgrades[(int)type]++;
        //Debug.Log(TowerManager.Instance.Upgrades[(int)type]);
        self.gameObject.GetComponentInChildren<Text>().text = TowerManager.Instance.GetUpgrade(type).ToString();
    }
}
