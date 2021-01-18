using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MonsterQuest
{
    public class FieldView : MonoBehaviour, IFieldView
    {
        [Inject]
        private IElementsViewSettings _elementsViewSettings;
        
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
            SpawnVisualElement( column,  row, _backgroundCell);
        }
        public void SpawnElement(int column, int row, Element element)
        {
           Image el =  _elementsViewSettings.ElementsImages[element];
           SpawnVisualElement(column, row, el.rectTransform);
        }

        private void SpawnVisualElement (int column,int row,RectTransform prefab)
        {
            
            RectTransform cell = Instantiate(prefab, _startPoint.position, Quaternion.identity, transform);
            cell.anchoredPosition = CalculateCellPosition(column, row);
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