using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public interface IScoreCounter
    {
        event Action<int> ScoreChanged;
        ScoreAmount SaveData { get; }
        void AddScore(HashSet<Vector2Int> deletedElements);
    }
}