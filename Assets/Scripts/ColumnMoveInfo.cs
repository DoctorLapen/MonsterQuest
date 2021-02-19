using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class ColumnMoveInfo
    {
        public int moveDistance;
        public List<Vector2Int> oldElements = new List<Vector2Int>();
        public List<NewElementInfo> newElements = new List<NewElementInfo>();
    }
}