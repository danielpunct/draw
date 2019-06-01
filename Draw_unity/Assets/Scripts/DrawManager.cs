using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public GameObject drawPrefab;

    public Sprite drawPlane;

    GameObject theTrail;

    public void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            var mouseRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mouseRay, Vector2.zero);
            if (hit.transform != null)
            {
                theTrail = Instantiate(drawPrefab, hit.collider.transform.position + (Vector3)hit.point + Vector3.back , Quaternion.identity);
            }
        }

        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
        {
            var mouseRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mouseRay, Vector2.zero);
            if (hit.transform != null)
            {
                theTrail.transform.position = hit.collider.transform.position + (Vector3)hit.point + Vector3.back;
            }
        }
    }
}