using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace MonsterQuest
{
    public class ScoreCounter : IScoreCounter
    {
        
        
        public event Action<int> ScoreChanged;

        public ScoreAmount SaveData => new ScoreAmount(_score);
        private int _oneElementCost;
        private int _Score
        {
            get { return _score; }

            set
            {
                _score = value;
                ScoreChanged?.Invoke(_score);
            }
        }

        private int _score;

        public ScoreCounter(int elementCost)
        {
            _oneElementCost = elementCost;
        }

        public void AddScore(HashSet<Vector2Int> deletedElements)
        {
            _Score += deletedElements.Count * _oneElementCost;
        }

    }
}