using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QuestionSystem
{

    public interface IRandomQuestion
    {
        string SetRandomQuestion(QuestionsObjectsHolder questionsObjectsHolder, TextMeshProUGUI text);
    }

    public class RandomQuestion : IRandomQuestion
    {
        private readonly QuestionsObjectsHolder _questionHolderObject;
        private readonly TextMeshProUGUI _text;
        
        private string currentTarget; 
        
        public RandomQuestion(QuestionsObjectsHolder questionHolderObject,TextMeshProUGUI text)
        {
            _questionHolderObject = questionHolderObject;
            _text = text;
        }

        public string SetRandomQuestion(QuestionsObjectsHolder questionsObjectsHolder, TextMeshProUGUI text)
        {
            if (questionsObjectsHolder == null || questionsObjectsHolder.QuestionsObjectsNames.Count == 0)
            {
                Debug.LogError("QuestionsObjectsHolder пустой или не назначен!");
                return "";
            }
            
            var questionGroups = questionsObjectsHolder.QuestionsObjectsNames
                .GroupBy(name => name)
                .Where(group => group.Count() == 1) // Оставляем только уникальные элементы
                .Select(group => group.Key)
                .ToList();
            
            if (questionGroups.Count == 0)
            {
                Debug.LogError("Нет уникальных вопросов в списке!");
                return "";
            }
            
            currentTarget = questionGroups[Random.Range(0, questionGroups.Count)];
            
            text.text = "Find: " + currentTarget;

            return currentTarget;
        }
    }
    
    [Serializable]
    public class GenerateQuestions : MonoBehaviour
    {
        [SerializeField] private QuestionsObjectsHolder _questionsHolder;
        [SerializeField] private TextMeshProUGUI _targetText;
        [SerializeField] private string _questionAnswer;
        [SerializeField] private GameObject _grid;

        private RandomQuestion _randomQuestion;

        public string QuestionAnswer => _questionAnswer;

        private void Awake()
        {
            ClearHolder();
        }

        private void Start()
        {
            _grid.transform.DOScale(0f, 0.1f)
                .SetEase(Ease.OutBounce)
                .OnComplete(() =>
                {
                    _grid.transform.DOScale(1.2f, 0.7f)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            _grid.transform.DOScale(1f, 1)
                                .SetEase(Ease.OutQuad);
                        });

                });
            
            _targetText.DOFade(1f, 2)
                .OnComplete(() =>
                {
                });
        }

        public void Initialize()
        {
            _randomQuestion = new RandomQuestion(_questionsHolder, _targetText);
            _questionAnswer = _randomQuestion.SetRandomQuestion(_questionsHolder,_targetText);
        }

        public void ClearHolder()
        {
            _questionsHolder.ClearHolder();
        }
        
    }
}
