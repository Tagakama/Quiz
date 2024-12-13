using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SettingGameloops", menuName = "Setting Game Loops/Settings Game Loop", order = 10)]
public class SettingGameLoops : ScriptableObject
{
   [SerializeField] private GameLoop[] _gameLoops;

   public GameLoop[] GameLoops => _gameLoops;
}
