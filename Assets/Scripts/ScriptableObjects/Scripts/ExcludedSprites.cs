using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ExcludedSpritesList", menuName = "Setting Game Loops/ExcludedSprites", order = 10)]
public class ExcludedSprites : ScriptableObject
{
   [SerializeField] private List<string> _excludedSprites;

   public List<string> ExcludedSprite => _excludedSprites;
}
