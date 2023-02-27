using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShadow : MonoBehaviour
{
    public int shadowOrder = -2;
    public int highlightOrder = -2;
    public int indicatorOrder = 1;
    GameObject shadow;
    public GameObject indicator;
    GameObject highlight;

    public Color shadowColor = new Color(30f/255, 30f/255, 30f/255, 1f);
    public Color highlightColor = new Color(1f, 1f, 0, 1f);
    Transform parentTr;

    public Vector3 shadowScale;
    public Vector3 highlightScale;
    public Vector3 indicatorScale;

    private Tower tower;
    void Start()
    {
        parentTr = gameObject.GetComponent<Transform>();

        shadow = AddSpriteFromResources("shadow", "characters/character_shadow", shadowColor, shadowOrder, shadowScale);
        highlight = AddSpriteFromResources("highlight", "characters/character_shadow", highlightColor, highlightOrder, highlightScale);
        highlight.transform.localPosition += Vector3.up * 0.2f;
        highlight.SetActive(false);

        indicator = AddSpriteFromResources("indicator", "characters/indicator", highlightColor, indicatorOrder, indicatorScale);
        indicator.transform.localPosition += Vector3.up * 0.85f;
        indicator.SetActive(false);

        tower = gameObject.GetComponent<Tower>();
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
                tower.GetComponent<CharacterShadow>().highlight.SetActive(onOff);
            }
        }
    }
}
