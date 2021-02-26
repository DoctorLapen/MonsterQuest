
using System;
using System.Collections;
using System.Collections.Generic;
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

        public event Action ColumnsMoved; 
        
        [SerializeField]
        private RectTransform _backgroundCell;

        [SerializeField]
        private RectTransform _startPoint;

        [SerializeField]
        private int _offsetBetweenCells;
        [SerializeField]
        private float _cellSize;

        [SerializeField]
        private float _moveStepY;

        

        
        
        private Vector3 _startPosition;

        private RectTransform[,] _elements;
        private float DistanceToMoveElementByY;
        private int _movingColumnsCount = 0;
        private int _currentMovedColums= 0;


        

        private void Start()
        {
            int columns = _levelData.Field.GetLength(0);
            int rows = _levelData.Field.GetLength(1);
            _elements = new RectTransform[columns, rows];
            _startPosition = _startPoint.anchoredPosition;
            Vector2 pointA = CalculateCellPosition(0, 0);
            Vector2 pointB = CalculateCellPosition(0, 1);
            DistanceToMoveElementByY = pointA.y - pointB.y;
        }

        public void SpawnBackgroundCell(int column,int row)
        {
            RectTransform cell = Instantiate(_backgroundCell, _startPoint.position, Quaternion.identity, transform);
            cell.sizeDelta = new Vector2(_cellSize, _cellSize);
            cell.anchoredPosition = CalculateCellPosition(column, row);
            cell.gameObject.GetComponent<CellCoordinate>().Initialize(new Vector2Int(column,row));
            cell.SetAsFirstSibling();
        }
        public void SpawnElement(int column, int row, Element element)
        {
            SpawnVisualElement(column, row, element, out RectTransform  cell);
           _elements[column, row] = cell;
        }

        private void SpawnVisualElement(int column, int row, Element element, out RectTransform cell)
        {
            Image el = _elementsViewSettings.ElementsImages[element];
            cell = Instantiate(el.rectTransform, _startPoint.position, Quaternion.identity, transform);
            cell.sizeDelta = new Vector2(_cellSize, _cellSize);
            cell.anchoredPosition = CalculateCellPosition(column, row);
            
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

        public void MoveDownElements(ElementsMoveDownArgs args)
        {
            _movingColumnsCount = 0;
            _currentMovedColums = 0;
            foreach (KeyValuePair<int,ColumnMoveInfo> moveInfoPair in args.columnsMoveInfos)
            {
                SpawnHiddenElements(moveInfoPair.Value.newElements, out List<HiddenElement> hiddenElements);
                StartCoroutine( MoveDownColumn( moveInfoPair.Value,hiddenElements));
                _movingColumnsCount++;
            }
          
        }

        private IEnumerator MoveDownColumn(ColumnMoveInfo moveInfo, List<HiddenElement> hiddenElements)
        {
            float moveDistance = DistanceToMoveElementByY * moveInfo.moveDistance;
            int steps =(int) (moveDistance / _moveStepY);
            float correctiveMoveStep = moveDistance - steps * _moveStepY ;
            int totalStepsAmount = steps + 1;
            int currentStepAmount = 0;
            float currentMoveStep = 0;
            while (currentStepAmount < totalStepsAmount)
            {
                if (currentStepAmount  < steps )
                {
                    currentMoveStep = _moveStepY;
                }
                else
                {
                    currentMoveStep = correctiveMoveStep;
                }

                foreach (Vector2Int elementCoordinate in moveInfo.oldElements)
                {
                    _elements[elementCoordinate.x, elementCoordinate.y].anchoredPosition -=
                        new Vector2(0, currentMoveStep);
                }
                foreach (HiddenElement element in hiddenElements )
                {
                    
                    element.Transform.anchoredPosition -=
                        new Vector2(0, currentMoveStep);
                }

                currentStepAmount++;
                yield return null;
            }

            foreach (Vector2Int elementCoordinate in moveInfo.oldElements)
            {
                RectTransform elementTransform = _elements[elementCoordinate.x, elementCoordinate.y];
                int newRowPosition = elementCoordinate.y + moveInfo.moveDistance;
                _elements[elementCoordinate.x, newRowPosition] = elementTransform;
            }
           
            foreach (HiddenElement element in hiddenElements)
            {
                int newRowPosition = element.Coordinate.y + moveInfo.moveDistance;
                _elements[element.Coordinate.x, newRowPosition] = element.Transform;
            }

            CountMovedColumn();
        }

        private void CountMovedColumn()
        {
            _currentMovedColums++;
            if (_currentMovedColums == _movingColumnsCount)
            {
                ColumnsMoved?.Invoke();
            }

        }

        private void SpawnHiddenElements(List<NewElementInfo> newElements,out List<HiddenElement> hiddenElements)
        {
            hiddenElements = new List<HiddenElement>();
            foreach (NewElementInfo newElement in newElements)
            {
                SpawnVisualElement(newElement.coordinate.x, newElement.coordinate.y, newElement.element, out RectTransform cell);
                HiddenElement hiddenElement = new HiddenElement(newElement.coordinate, cell);
                hiddenElements.Add(hiddenElement);
            }

        }
       



        private Vector3 CalculateCellPosition(int column, int row)
        {
            Vector3 newPosition =  _startPosition;
            newPosition.x += (_backgroundCell.sizeDelta.x + _offsetBetweenCells) * column;
            newPosition.y -= ( _backgroundCell.sizeDelta.y + _offsetBetweenCells) * row;
            return newPosition;
        }



        private class HiddenElement
        {
            public Vector2Int Coordinate { get;  }
            public RectTransform Transform { get; }
            public HiddenElement(Vector2Int coordinate, RectTransform transform)
            {
                Coordinate = coordinate;
                Transform = transform;
            }
        }

    }
}