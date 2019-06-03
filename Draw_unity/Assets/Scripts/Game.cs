using System.Collections;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public RenderTexture activeRT;
    public GameObject drawPrefab;
    public TMP_Text result;

    GameObject theTrail;

    int initialPatternCount;
    public float lineWeight = 0.22f;

    public Transform linesHolder;
    IEnumerator Start()
    {
        yield return  new WaitForSeconds(1);
       SampleTexture(true); 
    }

    public void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            var mouseRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mouseRay, Vector2.zero);
            if (hit.transform != null)
            {
                theTrail = Instantiate(drawPrefab, (Vector3) hit.point + Vector3.back,
                    Quaternion.identity, linesHolder);
                theTrail.layer = 8;
            }
        }

        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
        {
            var mouseRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mouseRay, Vector2.zero);
            if (hit.transform != null)
            {
                theTrail.transform.position = (Vector3)hit.point + Vector3.back;
            }
        }
        
        else if (Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
        {
           SampleTexture(false); 
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
                Debug.Log("initial " + initialPatternCount +" all: "+ pixels.Length);
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

            result.text = (drawPercent * 100).ToString("F2") + "%";
    }

    public void OnClearClick()
    {
        foreach (Transform child in linesHolder)
        {
            Destroy(child.gameObject);
        }
    }
}