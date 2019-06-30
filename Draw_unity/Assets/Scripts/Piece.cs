using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Piece : MonoBehaviour
{
  public GameObject outlineHolder;
  public GameObject coloredHolder;
  
  public Vector3 positionInFrame;
  public Vector3 rotationInFrame;
  public Vector3 scaleInFrame;

  public void ShowInit()
  {
    outlineHolder.SetActive(true);
    coloredHolder.SetActive(false);
  }

  public void ShowPassed()
  {
    coloredHolder.SetActive(true);
    coloredHolder.transform.SetParent(Game.Instance.frameHolder);
    var seq = DOTween.Sequence().AppendInterval(1)
      .Append(coloredHolder.transform.DOLocalMove(positionInFrame, 1f))
      .Append(coloredHolder.transform.DOScale(scaleInFrame, 1));
  }
}
