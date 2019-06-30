using System;
using System.Collections;
using Gamelogic.Extensions;
using TMPro;
using UnityEngine;

public class DrawBoard : Singleton<DrawBoard>
{
    public RenderTexture activeRT;
    public GameObject drawPrefab;
    public TMP_Text result;

    GameObject theTrail;

    int initialPatternCount;
    public float lineWeight = 0.22f;

    public Transform linesHolder;

    void Awake()
    {
        TouchHandler.ontouchPosition = OnFingerSet;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        SampleTexture(true);
    }

    void OnFingerSet(Vector2 screenPosition, TouchPhase phase)
    {
        switch (phase)
        {
            case TouchPhase.Began:
                var mouseRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var hit = Physics2D.Raycast(mouseRay, Vector2.zero);
                if (hit.transform != null)
                {
//                    Debug.Log(hit.point + " " + mouseRay);
                    theTrail = Instantiate(drawPrefab, (Vector3) hit.point + Vector3.back,
                        Quaternion.identity, linesHolder);
                    theTrail.layer = 8;
                }

//                    theTrail = Instantiate(drawPrefab, mouseRay + Vector3.forward * 9,
//                        Quaternion.identity, linesHolder);
//                    theTrail.layer = 8;
                break;

            case TouchPhase.Moved:
                mouseRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.Raycast(mouseRay, Vector2.zero);
                if (hit.transform != null)
                {
//                    Debug.Log(hit.point + " " + mouseRay);
                    theTrail.transform.position = (Vector3) hit.point + Vector3.back;
                }

//              theTrail.transform.position = (Vector3) mouseRay + Vector3.forward * 9;
                break;
            case TouchPhase.Ended:
                SampleTexture(false);
                break;
        }
    }

    void SampleTexture(bool initialSnapshot)
    {
        var t2d = new Texture2D(activeRT.width, activeRT.height, TextureFormat.RGB24, false);
        RenderTexture.active = activeRT;
        t2d.ReadPixels(new Rect(0, 0, t2d.width, t2d.height), 0, 0);

        var pixels = t2d.GetPixels();
        var patternCount = 0;
        var drawCount = 0;
        for (int i = 0; i < pixels.Length; i++)
        {
            patternCount += (pixels[i].r < 0.1f && pixels[i].g < 0.1f && pixels[i].b < 0.1f) ? 1 : 0;
            drawCount += (pixels[i].r > 0.9f && pixels[i].g < 0.1f && pixels[i].b < 0.1f) ? 1 : 0;
        }

        if (initialSnapshot)
        {
            initialPatternCount = patternCount;
            Debug.Log("initial " + initialPatternCount + " all: " + pixels.Length);
            return;
        }

        Debug.Log(patternCount + " " + drawCount);

        var imagePercent = initialPatternCount / (float) pixels.Length;
        var putPercent = drawCount * lineWeight / (float) pixels.Length;
        var drawPercent = 1 - patternCount / (float) initialPatternCount;
        var extraPercent = (drawCount * lineWeight - initialPatternCount) / pixels.Length;

        Debug.Log("image " + imagePercent + " put " + putPercent + " |   draw " + drawPercent + "   extra: " +
                  extraPercent);

        float resultValue = 0;

        if (putPercent <= 1)
        {
            resultValue = putPercent;
        }
        else
        {
            resultValue = Mathf.Max(0, 2 - putPercent);
        }

        //apply left percent
        resultValue *= (1 - drawPercent) / putPercent;

        result.text = (drawPercent * 100).ToString("F2") + "%" + " \n( don't tap anywhere !! )";

        if (drawPercent > 0.88f)
        {
            StartCoroutine(Game.Instance.ContourPassed());
        }
        else
        {
            StartCoroutine(Game.Instance.ContourFailed());
        }
    }

    public void ClearBoard()
    {
        linesHolder.DestroyChildren();
        result.text = "";
    }
}