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
        [Inject]
        private ILeveldData _levelData;
        
        [SerializeField]
        private RectTransform _backgroundCell;

        [SerializeField]
        private RectTransform _startPoint;

        [SerializeField]
        private int _offsetBetweenCells;
        [SerializeField]
        private float _cellSize;

        
        
        private Vector3 _startPosition;

        private RectTransform[,] _elements;


        

        private void Start()
        {
            int columns = _levelData.Field.GetLength(0);
            int rows = _levelData.Field.GetLength(1);
            _elements = new RectTransform[columns, rows];
            _startPosition = _startPoint.anchoredPosition;
        }

        public void SpawnBackgroundCell(int column,int row)
        {
            SpawnVisualElement( column,  row, _backgroundCell);
        }
        public void SpawnElement(int column, int row, Element element)
        {
           Image el = _elementsViewSettings.ElementsImages[element];
           RectTransform cell = Instantiate(el.rectTransform, _startPoint.position, Quaternion.identity, transform);
           cell.sizeDelta = new Vector2(_cellSize, _cellSize);
           cell.anchoredPosition = CalculateCellPosition(column, row);
           _elements[column, row] = cell;
        }

        private void SpawnVisualElement (int column,int row,RectTransform prefab)
        {
            
            RectTransform cell = Instantiate(prefab, _startPoint.position, Quaternion.identity, transform);
            cell.sizeDelta = new Vector2(_cellSize, _cellSize);
            cell.anchoredPosition = CalculateCellPosition(column, row);
            cell.gameObject.GetComponent<CellCoordinate>().Initialize(new Vector2Int(column,row));
            cell.SetAsFirstSibling();
        }

        public void ReplaceVisualElements(int columnA, int rowA, int columnB, int rowB)
        {
            RectTransform elementA = _elements[columnA,rowA];
            Vector2 positionA = elementA.anchoredPosition;
            RectTransform elementB = _elements[columnB,rowB];
            Vector2 positionB = elementB.anchoredPosition;
            elementA.anchoredPosition = positionB;
            elementB.anchoredPosition = positionA;
            _elements[columnA, rowA] = elementB;
            _elements[columnB, rowB] = elementA;



        }

        public void DeleteElement(int column, int row)
        {
            Destroy(_elements[column, row].gameObject);
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