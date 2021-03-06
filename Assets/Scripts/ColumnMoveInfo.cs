using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class ColumnMoveInfo
    {
        public List<SegmentMoveInfo> oldElements;
        public NewSegmentMoveInfo newElements;

        public ColumnMoveInfo()
        {
            this.oldElements = new List<SegmentMoveInfo>();
            newElements = new NewSegmentMoveInfo();
        }
        public ColumnMoveInfo(List<SegmentMoveInfo> oldElements)
        {
            this.oldElements = oldElements;
            newElements = new NewSegmentMoveInfo();
        }
    }
}