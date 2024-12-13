using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISpriteCheckService
{
    bool ShouldRotateSprite(Sprite sprite);
}

public interface ISpriteOrientationService
{
    void CorrectSpriteOrientation(SpriteRenderer spriteRenderer, Sprite sprite);
}

public class SpriteCheckService : ISpriteCheckService
{
    private readonly HashSet<string> _excludedSprites;

    public SpriteCheckService()
    {
        _excludedSprites = new HashSet<string> { "10", "M", "W" };
    }

    public bool ShouldRotateSprite(Sprite sprite)
    {
        if (_excludedSprites.Contains(sprite.name))
        {
            return false;
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

    public void CorrectSpriteOrientation(SpriteRenderer spriteRenderer, Sprite sprite)
    {
        if (_spriteCheckService.ShouldRotateSprite(sprite))
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

// Класс данных QuestObject
[Serializable]
public class QuestObject : MonoBehaviour
{
    [SerializeField] private Sprite _visualObject;
    [SerializeField] private string _nameObject;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private ISpriteOrientationService _orientationService;

    public string NameObject => _nameObject;
    public Sprite VisualObject => _visualObject;

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
            _orientationService.CorrectSpriteOrientation(_spriteRenderer, _visualObject);
        }
        else
        {
            Debug.LogWarning("Orientation service is not initialized.");
        }
    }
}