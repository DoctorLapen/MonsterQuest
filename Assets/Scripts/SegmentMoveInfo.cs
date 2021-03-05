using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class SegmentMoveInfo:IDistance
    {
        public int MoveDistance { get; set; }
        public List<Vector2Int> elementsToMove ;

        public SegmentMoveInfo(int moveDistance,List<Vector2Int> elementsToMove)
        {
            MoveDistance = moveDistance;
            this.elementsToMove = elementsToMove;
        }
    }
}