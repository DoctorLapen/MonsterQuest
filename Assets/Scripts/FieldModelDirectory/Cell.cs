using System;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public class Cell
    {
        public bool IsExist => _isExist;
        [SerializeField]
        private bool _isExist;

        public Element element;
        public bool isEmpty;
        public Cell()
        {
            
        }
        public Cell(bool isExist)
        {
             isEmpty = true;
            _isExist = isExist;
        }
        public Cell(bool isExist,Element element)
        {
            isEmpty = false;
            _isExist = isExist;
            this.element = element;
        }
    }


}