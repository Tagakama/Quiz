using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyValuePair<TKey, TValue>
{
   [SerializeField] private TKey _key;
   
   [SerializeField] private TValue _value;

   public TKey Key => _key;

   public TValue Value => _value;
}

[CreateAssetMenu(fileName = "New QuestionDataType", menuName = "Setting Game Loops/Question Data Type", order = 10)]
public class QuestionDataType : ScriptableObject
{
    [SerializeField] private List<KeyValuePair<Sprite,string>> _objectOfTheQuestion;

    public List<KeyValuePair<Sprite,string>> ObjectOfTheQuestion => _objectOfTheQuestion;
}
