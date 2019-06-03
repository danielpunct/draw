using System;
using UnityEngine;

public class Game : MonoBehaviour
{
   public Transform modelHolder;
   
   public Level[] levels;

   int currentLevel = 0;
   int currentModel = 0;
   
   void Start()
   {
      var currentModule = Instantiate(levels[currentLevel].models[currentModel], modelHolder.position, Quaternion.identity,
         modelHolder).GetComponent<Model>();
      
      currentModule.ShowInit();
   }
}
