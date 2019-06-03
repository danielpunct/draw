using System;
using System.Collections;
using Gamelogic.Extensions;
using UnityEngine;

public class Game : Singleton<Game>
{
   public Transform modelHolder;
   public Transform pictureHolder;
   
   public Level[] levels;

   int currentLevel = 0;
   int currentModel = 0;
   Model loadedModel;
   
   void Start()
   {
      LoadModel();
   }

   public IEnumerator ModelPassed()
   {
      loadedModel.ShowPassed();
      
      yield return  new WaitForSeconds(2);
      DrawBoard.Instance.OnClearClick();
      
      if (levels[currentLevel].models.Length == currentModel + 1)
      {
         if (levels.Length > currentLevel + 1)
         {
            currentLevel++;
            currentModel = 0;
         }
      }
      else
      {
         currentModel++;
      }
      
      LoadModel();
   }

   public IEnumerator ModelFailed()
   {
      yield return  new WaitForSeconds(1);
      DrawBoard.Instance.OnClearClick();
   }

   void LoadModel()
   {
      modelHolder.DestroyChildren();
      
      loadedModel = Instantiate(levels[currentLevel].models[currentModel], modelHolder.position, Quaternion.identity,
         modelHolder).GetComponent<Model>();
      
      loadedModel.ShowInit();
   }
}
