using System;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public class Cell
    {
        public bool IsEmpty => _isEmpty;
        [SerializeField]
        private bool _isEmpty;

        public Element element;
        public Cell()
        {
            
        }
        public Cell(bool isEmpty)
        {
            _isEmpty = isEmpty;
        }
    }


}