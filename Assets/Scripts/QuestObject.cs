using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestionSystem
{
    public interface ISpriteCheckService
    {
        bool ShouldRotateSprite(Sprite sprite ,string name);
    }

    public interface ISpriteOrientationService
    {
        void CorrectSpriteOrientation(SpriteRenderer spriteRenderer, Sprite sprite, string name);
    }

    public class SpriteCheckService : ISpriteCheckService
    {
        private readonly List<string> _excludedSprites;

        public SpriteCheckService(ExcludedSprites excludedSprites)
        {
            _excludedSprites = excludedSprites.ExcludedSprite;
        }

        public bool ShouldRotateSprite(Sprite sprite, string name)
        {
            foreach (var spriteName in _excludedSprites)
            {
                if (spriteName == name)
                {
                    return false;
                }
            }
            
            return sprite.rect.width > sprite.rect.height;
        }
    }

    public class SpriteOrientationService : ISpriteOrientationService
    {
        private readonly ISpriteCheckService _spriteCheckService;

        public SpriteOrientationService(ISpriteCheckService spriteCheckService)
        {
            _spriteCheckService = spriteCheckService;
        }

        public void CorrectSpriteOrientation(SpriteRenderer spriteRenderer, Sprite sprite, string name)
        {
            if (_spriteCheckService.ShouldRotateSprite(sprite,name))
            {
                spriteRenderer.transform.localRotation = Quaternion.Euler(0, 0, -90);
            }
            else
            {
                spriteRenderer.transform.localRotation = Quaternion.identity;
            }

            spriteRenderer.sprite = sprite;
        }
    }

    [Serializable]
    public class QuestObject : MonoBehaviour
    {
        [SerializeField] private Sprite _visualObject;
        [SerializeField] private string _nameObject;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private ISpriteOrientationService _orientationService;

        public string NameObject => _nameObject;
        public Sprite VisualObject => _visualObject;

        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        public void Initialize(Sprite visualObject, string nameObject, ISpriteOrientationService orientationService)
        {
            _visualObject = visualObject;
            _nameObject = nameObject;
            _orientationService = orientationService;
            ApplyVisualObject();
        }

        private void ApplyVisualObject()
        {
            if (_orientationService != null)
            {
                _orientationService.CorrectSpriteOrientation(_spriteRenderer, _visualObject, _nameObject);
            }
            else
            {
                Debug.LogWarning("Orientation service is not initialized.");
            }
        }
    }
}