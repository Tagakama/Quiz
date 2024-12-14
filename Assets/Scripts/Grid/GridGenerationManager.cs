using System;
using System.Collections.Generic;
using QuestionSystem;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Grid
{
    public class GridGenerationManager : MonoBehaviour
    {
        [SerializeField] private GameObject _questObjectPrefab;
        [SerializeField] private Transform _gridParent;
        [SerializeField] private SettingGameLoops _gameLoops;
        [SerializeField] private QuestionsObjectsHolder _questionsObjectsHolder;
        [SerializeField] private GenerateQuestions _generateQuestions;
        [SerializeField] private ExcludedSprites _excludedSprites;

        private List<GameObject> _activeHolders;

        private IGridGenerator _gridGenerator;
        private ILevelCatchData _levelCatchData;

        private void Start()
        {
            Initialize();
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

            _levelCatchData = new LevelCatchData(levels);

            var objectSelector = new ObjectSelector(_gameLoops);

            _gridGenerator = new GridGenerator(
                _questObjectPrefab,
                _gridParent,
                new QuestionObjectSaver(_questionsObjectsHolder),
                new ObjectDataHandler(objectSelector, _excludedSprites,_questionsObjectsHolder)
            );
            GenerateLevel();
        }

        public void GenerateLevel()
        {
            var currentLevel = _levelCatchData.GetCurrentLevelData();
            _gridGenerator.GenerateGrid(currentLevel);
            _levelCatchData.LoadNextLevel();
            _generateQuestions.Initialize();
        }

        public bool ItIsLastLevel()
        {
            return _levelCatchData.ItIsFinalLevel();
        }
        
        
    }
}
