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

    bool takeSnapshot;
    public void OnClickButton()
    {
        takeSnapshot = true;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (takeSnapshot)
        {
            var t2d = new Texture2D(src.width, src.width, TextureFormat.RGB24,false);
            RenderTexture.active = src;
            t2d.ReadPixels(new Rect(0, 0, t2d.width, t2d.height), 0, 0);
            
            var pixels = t2d.GetPixels();
            var count = 0;
            for (int i = 0; i < pixels.Length; i++)
            {
                count += (pixels[i].r != pixels[0].r || pixels[i].g != pixels[0].g && pixels[i].b != pixels[0].b) ? 1 : 0;
            }
            
            Debug.Log(count);
        }

        takeSnapshot = false;
        Graphics.Blit(src, dest);
    }
}