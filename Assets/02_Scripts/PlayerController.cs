using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    GameManager gm;
    ButtonManager bm;
    private void Start()
    {
        gm = GameManager.Instance;
        bm = ButtonManager.Instance;
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                
                switch (hit.collider.gameObject.layer)
                {
                    case 6:
                        Debug.Log("TowerArea");
                        gm.selected = GameManager.Selected.TOWER_AREA;
                        bm.MainBtn.onClick.RemoveAllListeners();
                        bm.MainBtn.onClick.AddListener(() => bm.OnBuildBtnClicked());
                        break;
                    case 7:
                        Debug.Log("Tower");
                        gm.selected = GameManager.Selected.TOWER;
                        bm.MainBtn.onClick.RemoveAllListeners();
                        bm.MainBtn.onClick.AddListener(() => bm.OnMergeBtnClicked(hit.collider.gameObject));
                        break;
                    case 8:
                        Debug.Log("Enemy");
                        gm.selected = GameManager.Selected.ENEMY;
                        bm.MainBtn.onClick.RemoveAllListeners();
                        break;
                    default:
                        break;
                }
                gm.selectedObject = hit.collider.gameObject;
                // UI 선택 대상 상태창 연동
            }
            else
            {
                bm.MainBtn.onClick.RemoveAllListeners();
                gm.selected = GameManager.Selected.NONE;
                gm.selectedObject = null;
            }
        }
    }
}
