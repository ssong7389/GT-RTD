using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShadow : MonoBehaviour
{
    public float productScaleX = 0.3f;
    public float productScaleY = 0.2f;
    public int order = -2;
    GameObject shadow;
    Transform shadowTr;
    SpriteRenderer shadowRenderer;
    public Color shadowColor = new Color(30f/255, 30f/255, 30f/255, 1f);

    Transform parentTr;
    Vector3 parentScale;
    void Start()
    {

        parentTr = gameObject.GetComponent<Transform>();

        shadow = new GameObject("shadow");
        shadowTr = shadow.GetComponent<Transform>();        
        parentScale = parentTr.localScale;
        shadowTr.position = parentTr.position;

        shadow.AddComponent<SpriteRenderer>();
        shadowRenderer = shadow.GetComponent<SpriteRenderer>();
        shadowRenderer.sprite = Resources.Load<Sprite>("characters/character_shadow");
        shadowRenderer.color = shadowColor;
        Debug.Log(shadowColor);
        Debug.Log(shadowRenderer.color);
        shadowRenderer.sortingOrder = order;
        shadowTr.localScale = new Vector3(parentScale.x * productScaleX, parentScale.y * productScaleY, parentScale.z);
        shadowTr.SetParent(parentTr);
    }
}
