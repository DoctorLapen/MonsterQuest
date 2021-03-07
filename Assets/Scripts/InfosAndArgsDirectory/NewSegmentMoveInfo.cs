using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class NewSegmentMoveInfo:IDistance
    {
        public int MoveDistance { get; set; }

        public List<NewElementInfo> elements = new List<NewElementInfo>();
    }
}