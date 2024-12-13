using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameLoop
{
    [SerializeField] private QuestionDataType[] _questionDataType;

    [SerializeField] private LevelData[] _levelData;

    public QuestionDataType[] QuestionDataType => _questionDataType;
    
    public LevelData[] LevelData => _levelData;
    
}
