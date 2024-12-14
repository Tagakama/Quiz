using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Grid;
using QuestionSystem;
using UnityEngine;
using UnityEngine.UI;

public class AnswerHandler : MonoBehaviour
{
    [SerializeField] private GenerateQuestions _generateQuestions;
    [SerializeField] private GridGenerationManager _gridGenerationManager;

    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Button _restartGameButton;
    private SpriteRenderer _answerSprite;
    
    
    public bool CheckAnswer(string answerName,SpriteRenderer sprite)
    {
        _answerSprite = sprite;
        
        if (_generateQuestions.QuestionAnswer == answerName)
        {
            GoodAnswer();
            return true;
        }

        BadAnswer();
        return false;
    }

    void GoodAnswer()
    {
        _answerSprite.transform.DOScale(0.06f, 1)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                _answerSprite.transform.DOScale(0.03f, 1)
                    .SetEase(Ease.OutQuad); 
                    LevelComplete();
            });
        
        
    }

    void BadAnswer()
    {
        _answerSprite.transform.DOLocalMoveX(-0.2f, 0.1f)
            .SetEase(Ease.InBounce)
            .OnComplete(() =>
            {
                _answerSprite.transform.DOLocalMoveX(0.2f, 0.1f) 
                    .SetEase(Ease.InBounce)
                    .OnComplete(() =>
                    {
                        _answerSprite.transform.DOLocalMoveX(0, 0.1f)
                            .SetEase(Ease.OutQuad);
                    });
            });
    }

    void LevelComplete()
    {
        if (_gridGenerationManager.ItIsLastLevel())
        {
            LastLevelComplete();
        }
        else
        {
            _generateQuestions.ClearHolder();
            _gridGenerationManager.GenerateLevel();
        }
    }

    void LastLevelComplete()
    {
        _backgroundImage.gameObject.SetActive(true);
        ImageFade(_backgroundImage, 0.7f, 2);
    }

    private void ImageFade(Image gameObject,float value,float duration)
    {
        gameObject.DOFade(value, duration)
            .OnComplete(() =>
            {
                _restartGameButton.gameObject.SetActive(true);
            });
    }
}
