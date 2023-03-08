using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TowerAreaIndicator : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Color originColor;
    public float originrgb;
    bool minus = true;
    public bool isIndicating = false;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originColor = spriteRenderer.material.color;
        originrgb = originColor.r;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IndicatesArea()
    {
        StartCoroutine(IndicatesAreaCoroutine());
    }
    public IEnumerator IndicatesAreaCoroutine()
    {
        if (!isIndicating)
        {
            isIndicating = true;
            while (GameManager.Instance.SelectedObject == gameObject)
            {

                if (minus)
                {
                    originrgb -= Time.deltaTime / 2;
                    if (originrgb < 0.5f)
                        minus = false;
                }
                else
                {
                    originrgb += Time.deltaTime / 2;
                    if (originrgb >= 1f)
                    {
                        minus = true;
                    }
                }
                spriteRenderer.material.color = new Color(originrgb, originrgb, originrgb, 1f);
                yield return new WaitForFixedUpdate();
            }
            spriteRenderer.material.color = Color.white;
            isIndicating = false;

        }            
    }
}
