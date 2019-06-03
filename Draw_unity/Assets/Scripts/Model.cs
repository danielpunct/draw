using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
  public GameObject outlineHolder;
  public GameObject coloredHolder;
  
  public Vector3 positionInFrame;
  public Vector3 rotationInFrame;

  public void ShowInit()
  {
    outlineHolder.SetActive(true);
    coloredHolder.SetActive(false);
  }
}
