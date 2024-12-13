using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New QuestionsObjectsHolder", menuName = "Setting Game Loops/New Questions Objects Holder", order = 10)]

public class QuestionsObjectsHolder : ScriptableObject
{
  [SerializeField] private List<string> _questionsObjectsNames;

  public List<string> QuestionsObjectsNames => _questionsObjectsNames;

  public void AddObjectName(string objectName)
  {
    _questionsObjectsNames.Add(objectName);
  }

}
