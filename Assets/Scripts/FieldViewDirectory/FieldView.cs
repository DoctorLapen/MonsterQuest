
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
                List<SegmentInfoInView> segmentInfos = new List<SegmentInfoInView>();
                foreach (SegmentMoveInfo segment in  moveInfoPair.Value.oldElements)
                {
                    SegmentInfoInView info = new SegmentInfoInView();
                    info.elements = GetOldElements(segment.elementsToMove);
                    info.moveDistance = segment.MoveDistance;
                    CalculateMoveProperties(segment.MoveDistance, info);
                    segmentInfos.Add(info);
                }

                SpawnHiddenElements(moveInfoPair.Value.newElements.elements, out List<MovingElement> hiddenElements);
                SegmentInfoInView newElementsSegment = new SegmentInfoInView();
                newElementsSegment.elements = hiddenElements;
                newElementsSegment.moveDistance = moveInfoPair.Value.newElements.MoveDistance;
                CalculateMoveProperties(moveInfoPair.Value.newElements.MoveDistance, newElementsSegment);
                segmentInfos.Add(newElementsSegment);
                StartCoroutine( MoveDownColumn(segmentInfos));
                _movingColumnsCount++;
            }
          
        }

        private IEnumerator MoveDownColumn(List<SegmentInfoInView> segmentInfos )
        {
            int segmentsMoved = 0;
            int totalSegmentsAmount = segmentInfos.Count;
            while (segmentsMoved < totalSegmentsAmount)
            {
                int i = 0 ;
                while (i < segmentInfos.Count)
                {
                    SegmentInfoInView segmentInfo = segmentInfos[i];
                    if (segmentInfo.CurrentStepAmount < segmentInfo.TotalMoveSteps)
                    {
                        float currentMoveStep = 0;
                        if (segmentInfo.CurrentStepAmount < segmentInfo.MoveSteps)
                        {
                            currentMoveStep = _moveStepY;
                        }
                        else
                        {
                            currentMoveStep = segmentInfo.correctiveMoveStep;
                        }
                        foreach (MovingElement movingElement in segmentInfo.elements)
                        {
                            movingElement.Transform.anchoredPosition -= new Vector2(0, currentMoveStep);
                        }

                        segmentInfo.CurrentStepAmount++;
                    }
                    else if (segmentInfo.CurrentStepAmount == segmentInfo.TotalMoveSteps && !segmentInfo.isMoved)
                    {
                        segmentsMoved++;
                        segmentInfo.isMoved = true;
                    }
                    i++;
                }

                yield return null;
            }

            foreach (SegmentInfoInView segmentInfo in segmentInfos)
            {
                Debug.Log($"segmentInfo.TotalAmount {segmentInfo.TotalMoveSteps}");
                foreach (MovingElement element in segmentInfo.elements)
                {
                    int newRowPosition = element.Coordinate.y + segmentInfo.moveDistance;
                    Debug.Log($"segmentInfo.moveDistance {segmentInfo.moveDistance}");
                    Debug.Log($"{element.Coordinate.x}  {newRowPosition}");
                    _elements[element.Coordinate.x, newRowPosition] = element.Transform;
                }
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

        private void SpawnHiddenElements(List<NewElementInfo> newElements,out List<MovingElement> hiddenElements)
        {
            hiddenElements = new List<MovingElement>();
            foreach (NewElementInfo newElement in newElements)
            {
                SpawnVisualElement(newElement.coordinate.x, newElement.coordinate.y, newElement.element, out RectTransform cell);
                MovingElement movingElement = new MovingElement(newElement.coordinate, cell);
                hiddenElements.Add(movingElement);
            }

        }



        private List<MovingElement> GetOldElements(List<Vector2Int> coordinates)
        {
            List<MovingElement> oldElements = new List<MovingElement>();
            foreach (var coord in coordinates)
            {
                RectTransform elementTransform = _elements[coord.x, coord.y];
                MovingElement oldElement = new MovingElement(coord,elementTransform );
                oldElements.Add(oldElement);
            }

            return oldElements;
        }
        private void CalculateMoveProperties(int moveDistance,SegmentInfoInView segmentInfo)
        {
            float distance = DistanceToMoveElementByY * moveDistance;
            segmentInfo.MoveSteps =(int) (distance / _moveStepY);
            segmentInfo.correctiveMoveStep = distance  - segmentInfo.MoveSteps * _moveStepY ;
           
        }
        private Vector3 CalculateCellPosition(int column, int row)
        {
            Vector3 newPosition =  _startPosition;
            newPosition.x += (_backgroundCell.sizeDelta.x + _offsetBetweenCells) * column;
            newPosition.y -= ( _backgroundCell.sizeDelta.y + _offsetBetweenCells) * row;
            return newPosition;
        }


        private class SegmentInfoInView
        {
            public int TotalMoveSteps => MoveSteps + 1;
            public int MoveSteps { get; set; }
            public int CurrentStepAmount { get; set; } = 0;
            public float correctiveMoveStep;
            public List<MovingElement> elements;
            public int moveDistance;
            public bool isMoved = false;


        }

        private class MovingElement
        {
            public Vector2Int Coordinate { get;  }
            public RectTransform Transform { get; }
            public MovingElement(Vector2Int coordinate, RectTransform transform)
            {
                Coordinate = coordinate;
                Transform = transform;
            }
        }

    }
}