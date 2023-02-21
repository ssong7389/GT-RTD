using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    GameManager gm;
    ButtonManager bm;
    Camera cam;
    public float camLimit = 5f;

    private float Speed = 0.25f;
    private Vector2 nowPos, prePos;
    private Vector3 movePos;
    public float zoomSpeed = 0.25f;
    private void Start()
    {
        gm = GameManager.Instance;
        bm = ButtonManager.Instance;
        cam = Camera.main;
    }
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                prePos = touch.position - touch.deltaPosition;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                nowPos = touch.position - touch.deltaPosition;
                movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * Speed;
                cam.transform.Translate(movePos);
                prePos = touch.position - touch.deltaPosition;
            }
            // camera 이동 범위 제한
        }
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            Vector2 touch1PrePos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrePos = touch2.position - touch2.deltaPosition;

            float preTouchDeltaMag = (touch1PrePos - touch2PrePos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            float deltaMagnitudeDiff = preTouchDeltaMag - touchDeltaMag;

            Camera.main.orthographicSize += deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;
            Camera.main.orthographicSize = Mathf.Max(cam.orthographicSize, 0.1f);

            if (cam.orthographicSize > 7)
                cam.orthographicSize = 7;

            if (cam.orthographicSize < 1)
                cam.orthographicSize = 1;
        }
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
