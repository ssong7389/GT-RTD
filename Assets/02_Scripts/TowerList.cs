using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TowerList : MonoBehaviour
{
    public GameObject infoPrefab;
    Tower[] towers;
    public string rank;

    SpriteAtlas characterAtlas;
    void Start()
    {
        characterAtlas = Resources.Load<SpriteAtlas>("characters/characters_atlas");
        towers = Resources.LoadAll<Tower>(rank);
        foreach (var tower in towers)
        {
            GameObject info = Instantiate(infoPrefab);
            Text towerName = info.transform.Find("Name").GetComponent<Text>();
            Image portrait = info.transform.Find("Portrait").GetComponent<Image>();
            Image typeinfo = info.transform.Find("Type").GetComponent<Image>();
            Text damageinfo = info.transform.Find("Dmg").GetComponent<Text>();
            Text speedinfo = info.transform.Find("Speed").GetComponent<Text>();
            towerName.text = tower.TowerName;
            portrait.sprite = characterAtlas.GetSprite(tower.portraitName);
            portrait.SetNativeSize();
            float width = portrait.rectTransform.rect.width;
            float height = portrait.rectTransform.rect.height;
            portrait.rectTransform.sizeDelta = new Vector2(width * 2f, height * 2f);
            typeinfo.sprite = characterAtlas.GetSprite($"upgrade_{tower.Type}");
            damageinfo.text = tower.dmg.ToString();
            speedinfo.text = tower.attackSpeed.ToString();

            info.transform.SetParent(transform);
        }
    }

}
