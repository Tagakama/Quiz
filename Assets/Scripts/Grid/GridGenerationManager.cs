using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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

    public interface ILevelManager
    {
        ILevelData GetCurrentLevelData();
        void LoadNextLevel();
    }

    public class LevelManager : ILevelManager
    {
        private readonly IList<ILevelData> _levels;
        private int _currentLevelIndex;

        public LevelManager(IList<ILevelData> levels)
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
    }

    public interface IGridGenerator
    {
        void GenerateGrid(ILevelData levelData);
    }

    public class GridGenerator : IGridGenerator
    {
        private readonly GameObject _prefab;
        private readonly Transform _parentTransform;
        private readonly IQuestionObjectSaver _questionObjectSaver;
        private readonly ISettingObjectData _settingObjectData;

        public GridGenerator(GameObject prefab, Transform parentTransform, IQuestionObjectSaver questionObjectSaver, ISettingObjectData settingObjectData)
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
                        column * _prefab.transform.localScale.y,
                        row * _prefab.transform.localScale.x,
                        0
                    );

                    GameObject newObject = Object.Instantiate(_prefab, position, Quaternion.identity, _parentTransform);
                    _settingObjectData.SetObjectData(newObject);
                    _questionObjectSaver.Save(newObject);
                }
            }
        }
    }

    public interface IObjectSelector
    {
        KeyValuePair<Sprite, string> GetRandomObject();
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

    public class SetterObjectData : ISettingObjectData
    {
        private readonly IObjectSelector _objectSelector;

        public SetterObjectData(IObjectSelector objectSelector)
        {
            _objectSelector = objectSelector;
        }

        public void SetObjectData(GameObject questionGameObject)
        {
            var randomItem = _objectSelector.GetRandomObject();
            QuestObject questObject = questionGameObject.GetComponent<QuestObject>();
            questObject.Initialize(
                randomItem.Key,
                randomItem.Value,
                new SpriteOrientationService(new SpriteCheckService())
            );
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
            var questObject = questionGameObject.GetComponent<QuestObject>();
            if (questObject != null)
            {
                if (_checkForRepeatName.IsNameUnique(questObject.NameObject))
                {
                    _holder.AddObjectName(questObject.NameObject);
                    Debug.Log($"Saved object: {questObject.NameObject}");
                }
                else
                {
                    Debug.LogWarning($"Duplicate object name: {questObject.NameObject}");
                }
            }
        }
    }

    public class GridGenerationManager : MonoBehaviour
    {
        [SerializeField] private GameObject _questObjectPrefab;
        [SerializeField] private Transform _gridParent;
        [SerializeField] private SettingGameLoops _gameLoops;
        [SerializeField] private QuestionsObjectsHolder _questionsObjectsHolder;

        private IGridGenerator _gridGenerator;
        private ILevelManager _levelManager;

        private void Start()
        {
            Initialize();
            GenerateLevel();
        }

        private void Initialize()
        {
            var levels = new List<ILevelData>();
            foreach (var gameLoop in _gameLoops.GameLoops)
            {
                foreach (var level in gameLoop.LevelData)
                {
                    levels.Add(new LevelData(level.NumberOfLines, level.NumderOfColumns));
                }
            }

            _levelManager = new LevelManager(levels);

            var objectSelector = new ObjectSelector(_gameLoops);

            _gridGenerator = new GridGenerator(
                _questObjectPrefab,
                _gridParent,
                new QuestionObjectSaver(_questionsObjectsHolder),
                new SetterObjectData(objectSelector)
            );
        }

        private void GenerateLevel()
        {
            var currentLevel = _levelManager.GetCurrentLevelData();
            _gridGenerator.GenerateGrid(currentLevel);
            _levelManager.LoadNextLevel();
        }
    }
}
