using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QuestionSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "New QuestionsObjectsHolder", menuName = "Setting Game Loops/New Questions Objects Holder", order = 10)]

public class QuestionsObjectsHolder : ScriptableObject
{
  [SerializeField] private List<string> _questionsObjectsNames;
  [SerializeField] private List<GameObject> _questionsGameObjects;

  public List<string> QuestionsObjectsNames => _questionsObjectsNames;

  public void AddObjectName(string objectName)
  {
    _questionsObjectsNames.Add(objectName);
  }

  public void AddGameObject(GameObject gameObject)
  {
    _questionsGameObjects.Add(gameObject);
  }

  public void ClearHolder()
  {
    _questionsObjectsNames.Clear();
    
    foreach (var obj in _questionsGameObjects)
    {
      if (obj != null)
      {
       
        var questObject = obj.GetComponent<QuestObject>();
        if (questObject != null && questObject.SpriteRenderer != null)
        {
         
          DOTween.Kill(questObject.SpriteRenderer.transform);
        }
        
        Destroy(obj);
      }
    }
    _questionsGameObjects.Clear();
  }

}
