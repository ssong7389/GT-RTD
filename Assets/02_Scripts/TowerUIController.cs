using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUIController : MonoBehaviour
{
    public int shadowOrder = -1;
    public int highlightOrder = -2;
    public int indicatorOrder = 1;
    public int rangeOrder = -3;
    GameObject shadow;
    public GameObject indicator;
    GameObject highlight;
    GameObject range;

    public Color shadowColor = new Color(30f/255, 30f/255, 30f/255, 1f);
    public Color highlightColor = new Color(1f, 1f, 0, 1f);
    public Color rangeColor = new Color(1f, 30f/255, 0f, 150f/255);
    Transform parentTr;

    public Vector3 shadowScale = new Vector3(0.3f, 0.2f, 1f);
    public Vector3 highlightScale = new Vector3(0.6f, 0.6f, 1f);
    public Vector3 indicatorScale = new Vector3(1f, -1f, 1f);

    private Tower tower;
    void Start()
    {
        parentTr = gameObject.GetComponent<Transform>();

        shadow = AddSpriteFromResources("shadow", "characters/character_shadow", shadowColor, shadowOrder, shadowScale);
        highlight = AddSpriteFromResources("highlight", "characters/character_shadow", highlightColor, highlightOrder, highlightScale);
        highlight.transform.localPosition += Vector3.up * 0.2f;
        highlight.SetActive(false);

        indicator = AddSpriteFromResources("indicator", "characters/ui_indicator", Color.white, indicatorOrder, indicatorScale);
        indicator.transform.localPosition += Vector3.up * 0.85f;
        indicator.SetActive(false);

        // 사거리 x 2
        tower = gameObject.GetComponent<Tower>();
        float scaleX = tower.range * 0.8f;
        Vector3 rangeScale = new Vector3(scaleX, scaleX, 1f);
        range = AddSpriteFromResources("range", "characters/range", rangeColor, rangeOrder, rangeScale);
        range.transform.localPosition += Vector3.up * 0.1f;
        range.SetActive(false);
    }
    private void Update()
    {
        if (!indicator.activeSelf)
            return;
        if (GameManager.Instance.SelectedObject != gameObject)
        {
                IndicatesTower(false);
        }
    }
    GameObject AddSpriteFromResources(string name, string path, Color color, int order, Vector3 childScale)
    {
        GameObject obj = new GameObject(name);
        Transform tr = obj.GetComponent<Transform>();
        tr.position = parentTr.position;
        obj.AddComponent<SpriteRenderer>();

        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>(path);
        
        renderer.color = color;
        renderer.sortingOrder = order;
        tr.SetParent(parentTr);
        tr.localScale = childScale;

        return obj;
    }
    public void IndicatesTower(bool onOff)
    {

        indicator.SetActive(onOff);
        range.SetActive(onOff);
        // 같은 타워면 인디케이터만 끄도록
        GameObject selectedObj = GameManager.Instance.SelectedObject;
        if ( selectedObj!= gameObject && selectedObj?.name == gameObject.name)
        {
            return;
        }
        else
        {
            HighlightsTowers(onOff);
        }
        
    }
    public void HighlightsTowers(bool onOff)
    {
        GameObject[] rankTowers = GameObject.FindGameObjectsWithTag(tower.Rank.ToString());

        foreach (var tower in rankTowers)
        {
            if (tower.name == gameObject.name)
            {
                tower.GetComponent<TowerUIController>().highlight.SetActive(onOff);
            }
        }
    }
}
