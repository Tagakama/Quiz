using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public interface IQuestionObjectSaver
    {
        void Save(GameObject questionGameObject);
    }

    public interface ISettingObjectData
    {
        void SetObjectData(GameObject questionGameObject);
    }

    public interface ILevelData
    {
        int Rows { get; }
        int Columns { get; }
    }
    
    public interface ILevelCatchData
    {
        ILevelData GetCurrentLevelData();
        void LoadNextLevel();
        bool ItIsFinalLevel();

    }
    
    public interface IGridGenerator
    {
        void GenerateGrid(ILevelData levelData);
    }
    
    public interface IObjectSelector
    {
        KeyValuePair<Sprite, string> GetRandomObject();
    }
}

