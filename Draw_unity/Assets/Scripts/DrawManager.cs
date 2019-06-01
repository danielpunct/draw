using TMPro;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public GameObject drawPrefab;
    public TMP_Text result;

    GameObject theTrail;

    int initialPatternCount;
    public float lineWeight = 0.22f;

    public Transform linesHolder;
    void Start()
    {
        initialSnapshot = true;
    }

    public void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            var mouseRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mouseRay, Vector2.zero);
            if (hit.transform != null)
            {
                theTrail = Instantiate(drawPrefab, hit.collider.transform.position + (Vector3) hit.point + Vector3.back,
                    Quaternion.identity, linesHolder);
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
        
        else if (Input.touchCount> 0 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
        {
            takeSnapshot = true;
        }
    }

    bool takeSnapshot;
    bool initialSnapshot;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (takeSnapshot || initialSnapshot)
        {
            var t2d = new Texture2D(src.width, src.height, TextureFormat.RGB24,false);
            RenderTexture.active = src;
            t2d.ReadPixels(new Rect(0, 0, t2d.width, t2d.height), 0, 0);
            
            var pixels = t2d.GetPixels();
            var patternCount = 0;
            var drawCount = 0;
            for (int i = 0; i < pixels.Length; i++)
            {
                patternCount += (pixels[i].r < 0.1f && pixels[i].g < 0.1f && pixels[i].b < 0.1f) ? 1 : 0;
                drawCount += (pixels[i].r >0.9f && pixels[i].g < 0.1f && pixels[i].b < 0.1f) ? 1 : 0;
            }

            if (initialSnapshot)
            {
                initialPatternCount = patternCount;
            }
            
            Debug.Log(patternCount + " "+drawCount);
            

            var percent = drawCount * lineWeight / initialPatternCount;
            
            Debug.Log("result "+ percent);

            float resultValue = 0;

            if (percent <= 1)
            {
                resultValue = percent;
            }
            else
            {
                resultValue = Mathf.Max(0, 2 - percent);
            }

            result.text = (resultValue * 100).ToString("F2") + "%";
        }

        takeSnapshot = false;
        initialSnapshot = false;
        Graphics.Blit(src, dest);
    }

    public void OnClearClick()
    {
        foreach (Transform child in linesHolder)
        {
            Destroy(child.gameObject);
        }
    }
}