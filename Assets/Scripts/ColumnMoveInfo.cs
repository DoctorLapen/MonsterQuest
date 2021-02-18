using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class ColumnMoveInfo
    {
        public int moveDistance;
        public List<Vector2Int> elementsToMove = new List<Vector2Int>();
    }
}