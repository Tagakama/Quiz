using System.Collections;
using System.Collections.Generic;
using QuestionSystem;
using UnityEngine;

namespace Grid
{
    public class LevelData : ILevelData
    {
        public int Rows { get; }
        public int Columns { get; }

        public LevelData(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }
    }
    
    public class LevelCatchData : ILevelCatchData
    {
        private readonly IList<ILevelData> _levels;
        private int _currentLevelIndex;

        public LevelCatchData(IList<ILevelData> levels)
        {
            _levels = levels;
        }

        public ILevelData GetCurrentLevelData()
        {
            return _levels[_currentLevelIndex];
        }

        public void LoadNextLevel()
        {
            _currentLevelIndex = (_currentLevelIndex + 1) % _levels.Count;
        }
        
        public bool ItIsFinalLevel()
        {
            if (_currentLevelIndex == 0)
            {
                return true;
            }
            return false;
        }
    }
    
    public class GridGenerator : IGridGenerator
    {
        private readonly GameObject _prefab;
        private readonly Transform _parentTransform;
        private readonly IQuestionObjectSaver _questionObjectSaver;
        private readonly ISettingObjectData _settingObjectData;

        public GridGenerator(GameObject prefab, Transform parentTransform,
            IQuestionObjectSaver questionObjectSaver, ISettingObjectData settingObjectData)
        {
            _prefab = prefab;
            _parentTransform = parentTransform;
            _questionObjectSaver = questionObjectSaver;
            _settingObjectData = settingObjectData;
        }

        public void GenerateGrid(ILevelData levelData)
        {
            for (int row = 0; row < levelData.Rows; row++)
            {
                for (int column = 0; column < levelData.Columns; column++)
                {
                    Vector3 position = new Vector3(
                        column * _prefab.transform.localScale.y - _prefab.transform.localScale.x,
                        row * _prefab.transform.localScale.x ,
                        10
                    );

                    GameObject newObject = Object.Instantiate(_prefab, position, Quaternion.identity, _parentTransform);
                    _settingObjectData.SetObjectData(newObject);
                }
            }
        }
    }
    
    public class ObjectSelector : IObjectSelector
    {
        private readonly SettingGameLoops _gameLoops;
        private readonly GameLoop _selectedGameLoop;
        private List<KeyValuePair<Sprite, string>> _typeObjectListHolder;

        public ObjectSelector(SettingGameLoops gameLoops)
        {
            _gameLoops = gameLoops;
            _selectedGameLoop = gameLoops.GameLoops[Random.Range(0, gameLoops.GameLoops.Length)];
        }

        public KeyValuePair<Sprite, string> GetRandomObject()
        {
            if (_typeObjectListHolder == null)
            {
                _typeObjectListHolder = _selectedGameLoop
                    .QuestionDataType[Random.Range(0, _selectedGameLoop.QuestionDataType.Length)]
                    .ObjectOfTheQuestion;
            }

            return _typeObjectListHolder[Random.Range(0, _typeObjectListHolder.Count)];
        }
    }

    public class ObjectDataHandler : ISettingObjectData
    {
        private readonly IObjectSelector _objectSelector;
        private readonly ExcludedSprites _excludedSprites;
        private readonly QuestionsObjectsHolder _holder;
        private readonly HashSet<string> _checkedNames;

        public ObjectDataHandler(IObjectSelector objectSelector, ExcludedSprites excludedSprites, QuestionsObjectsHolder holder)
        {
            _objectSelector = objectSelector;
            _excludedSprites = excludedSprites;
            _holder = holder;
            _checkedNames = new HashSet<string>();
        }

        public void SetObjectData(GameObject questionGameObject)
        {
            for (int i = 0; i < 10; i++)
            {
                var randomItem = _objectSelector.GetRandomObject();
                
                    QuestObject questObject = questionGameObject.GetComponent<QuestObject>();
                    questObject.Initialize(
                        randomItem.Key,
                        randomItem.Value,
                        new SpriteOrientationService(new SpriteCheckService(_excludedSprites))
                    );
                    Save(questionGameObject);
                    break;
            }
        }

        private bool IsNameUnique(string name)
        {
            if (_checkedNames.Contains(name))
            {
                return false;
            }

            _checkedNames.Add(name);
            return true;
        }

        private void Save(GameObject questionGameObject)
        {
            _holder.AddGameObject(questionGameObject);
            var questObject = questionGameObject.GetComponent<QuestObject>();
            _holder.AddObjectName(questObject.NameObject);
        }
    }

    public class CheckForRepeatName
    {
        private readonly HashSet<string> _checkedNames;

        public CheckForRepeatName()
        {
            _checkedNames = new HashSet<string>();
        }

        public bool IsNameUnique(string name)
        {
            if (_checkedNames.Contains(name))
            {
                return false;
            }

            _checkedNames.Add(name);
            return true;
        }
    }

    public class QuestionObjectSaver : IQuestionObjectSaver
    {
        private readonly QuestionsObjectsHolder _holder;
        private readonly CheckForRepeatName _checkForRepeatName;

        public QuestionObjectSaver(QuestionsObjectsHolder holder)
        {
            _holder = holder;
            _checkForRepeatName = new CheckForRepeatName();
        }

        public void Save(GameObject questionGameObject)
        {
            _holder.AddGameObject(questionGameObject);
            var questObject = questionGameObject.GetComponent<QuestObject>();
            if (questObject != null)
            {
                if (_checkForRepeatName.IsNameUnique(questObject.NameObject))
                {
                    _holder.AddObjectName(questObject.NameObject);
                }
                else
                {
                    Debug.LogWarning($"Duplicate object name: {questObject.NameObject}");
                }
            }
        }
    }
}

