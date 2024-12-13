using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
   [SerializeField] private int _numberOfLines;

   [SerializeField] private int _numderOfColumns;

   public int NumberOfLines => _numberOfLines;
   
   public int NumderOfColumns => _numderOfColumns;
}
