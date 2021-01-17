using System;
using UnityEngine;

namespace MonsterQuest
{
    public class FieldView : MonoBehaviour, IFieldView
    {
        [SerializeField]
        private RectTransform _backgroundCell;

        [SerializeField]
        private RectTransform _startPoint;

        [SerializeField]
        private int _offsetBetweenCells;

       

        

        
        private Vector3 _startPosition;


        

        private void Start()
        {
            _startPosition = _startPoint.anchoredPosition;
        }

        public void SpawnBackgroundCell(int column,int row)
        {
            RectTransform cell = Instantiate(_backgroundCell, _startPoint.position, Quaternion.identity, transform);
            cell.anchoredPosition = CalculateCellPosition(column, row);
        }
        public void SpawnElement(int column,int row)
        {
            
        }
    

        
        private Vector3 CalculateCellPosition(int column, int row)
        {
            Vector3 newPosition =  _startPosition;
            newPosition.x += (_backgroundCell.sizeDelta.x + _offsetBetweenCells) * column;
            newPosition.y -= ( _backgroundCell.sizeDelta.y + _offsetBetweenCells) * row;
            return newPosition;
        }
        
    }
}