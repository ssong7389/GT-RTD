using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    GameManager gm;
    ButtonManager bm;
    Camera cam;
    Transform camTr;
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
        camTr = cam.transform;
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

                camTr.Translate(movePos);
                prePos = touch.position - touch.deltaPosition;

                if (camTr.position.x < -camLimit)
                    camTr.position = new Vector3(-camLimit, camTr.position.y, camTr.position.z);
                if (camTr.position.x > camLimit)
                    camTr.position = new Vector3(camLimit, camTr.position.y, camTr.position.z);
                if (camTr.position.y < -camLimit)
                    camTr.position = new Vector3(camTr.position.x, -camLimit, camTr.position.z);
                if (camTr.position.y > camLimit)
                    camTr.position = new Vector3(camTr.position.x, camLimit, camTr.position.z);
            }
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

            cam.orthographicSize += deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;
            cam.orthographicSize = Mathf.Max(cam.orthographicSize, 0.1f);

            if (cam.orthographicSize > 7)
                cam.orthographicSize = 7;

            if (cam.orthographicSize < 1)
                cam.orthographicSize = 1;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
                GameManager.Instance.SelectedObject = hit.collider.gameObject;
            }
            else
            {                
                gm.SelectedObject = null;
            }
        }
    }
}
