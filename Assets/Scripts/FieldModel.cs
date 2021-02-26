using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MonsterQuest
{
    public class FieldModel : IFieldModel
    {
        public event Action<CellChangedArgs> CellChanged;
        public event Action<ElementsReplacedArgs> ElementsReplaced;
        public event Action<ElementsMoveDownArgs> ElementsMovedDown;
        private const int MIN_MATCHES = 3;
        private readonly Cell[,] _field;
        private readonly int _fieldColumns;
        private readonly int _fieldRows;
        private ILeveldData _levelData;

        public FieldModel(ILeveldData levelData)
        {
            _levelData = levelData;
            _field = levelData.Field;
            _fieldColumns = levelData.Field.GetLength(0);
            _fieldRows = levelData.Field.GetLength(1);
        }
        

        public void InitializeField()
        {
            var columnPreviusElements = new List<Element>();
            var rowPreviusElements = new List<Element>();
            for (var column = 0; column < _fieldColumns; column++)
            {
                for (var row = 0; row < _fieldRows; row++)
                {
                    var cell = _field[column, row];
                    if (!cell.IsExist)
                    {
                        var currentElement = Element.Yellow;

                        if (column > 1 && row > 1)
                        {
                            columnPreviusElements.Add(_field[column - 2, row].element);
                            columnPreviusElements.Add(_field[column - 1, row].element);
                            rowPreviusElements.Add(_field[column, row - 2].element);
                            rowPreviusElements.Add(_field[column, row - 1].element);
                            currentElement = SelectSuitableElement(columnPreviusElements, rowPreviusElements);
                        }
                        else if (column > 1)
                        {
                            columnPreviusElements.Add(_field[column - 2, row].element);
                            columnPreviusElements.Add(_field[column - 1, row].element);
                            currentElement = SelectSuitableElement(columnPreviusElements, rowPreviusElements);
                        }
                        else if (row > 1)
                        {
                            rowPreviusElements.Add(_field[column, row - 2].element);
                            rowPreviusElements.Add(_field[column, row - 1].element);
                            currentElement = SelectSuitableElement(columnPreviusElements, rowPreviusElements);
                        }
                        else
                        {
                            currentElement = SelectRandomElement();
                        }

                        cell.element = currentElement;
                        SendCellInfo(column, row, cell.element, ChangeType.Initialize);
                    }
                }
            }
        }

        public void ShiftElements()
        {
            Dictionary<int,ColumnMoveInfo> columnMoveInfos = FillEmptyCells();
           AddNewElements(columnMoveInfos );
           ElementsMoveDownArgs args = new ElementsMoveDownArgs();
           args.columnsMoveInfos = columnMoveInfos;
           ElementsMovedDown?.Invoke(args);
        }

        public bool IsElementInField(Vector2Int elementCoordinates)
        {
            var isXInField = 0 <= elementCoordinates.x && elementCoordinates.x < _fieldColumns;
            var isYInField = 0 <= elementCoordinates.y && elementCoordinates.y < _fieldRows;
            return isXInField && isYInField;
        }
        public HashSet<Vector2Int> FindMatchedElements()
        {
            return  FindMatch(_field);
        }
        public HashSet<Vector2Int> FindMatchedElements(Vector2Int coordinateA, Vector2Int coordinateB)
        {
            var _testField = new Cell[_fieldColumns, _fieldRows];
            Array.Copy(_field, _testField, _field.Length);
            var elementA = _testField[coordinateA.x, coordinateA.y];
            var elementB = _testField[coordinateB.x, coordinateB.y];
            _testField[coordinateA.x, coordinateA.y] = elementB;
            _testField[coordinateB.x, coordinateB.y] = elementA;
            return FindMatch(_testField);
        }

        private HashSet<Vector2Int> FindMatch(Cell[,] field)
        {
            var matchedHorizontalElements = FindMatchesByDirection(_fieldRows, _fieldColumns,
                (j, i) => field[j, i].element, (j, i) => new Vector2Int(j, i));
            var matchedVerticalElements = FindMatchesByDirection(_fieldColumns, _fieldRows, (j, i) => field[i, j].element,
                (j, i) => new Vector2Int(i, j));
            matchedHorizontalElements.UnionWith(matchedVerticalElements);
            
            return matchedHorizontalElements;
        }

        public void ReplaceElements(Vector2Int coordinatesA, Vector2Int coordinatesB)
        {
            var elementA = _field[coordinatesA.x, coordinatesA.y];
            var elementB = _field[coordinatesB.x, coordinatesB.y];
            _field[coordinatesA.x, coordinatesA.y] = elementB;
            _field[coordinatesB.x, coordinatesB.y] = elementA;
            var args = new ElementsReplacedArgs();
            args.elementA = coordinatesA;
            args.elementB = coordinatesB;
            ElementsReplaced?.Invoke(args);
        }

        public void DeleteElements(IEnumerable<Vector2Int> elementsCoordinates)
        {
            foreach (var coordinate in elementsCoordinates)
            {
                _field[coordinate.x, coordinate.y].isEmpty = true;
                SendCellInfo(coordinate.x, coordinate.y, Element.Yellow, ChangeType.Delete);
            }
        }

        public Dictionary<int,ColumnMoveInfo> FillEmptyCells()
        {
            Dictionary<int,ColumnMoveInfo> columnMoveInfos = new  Dictionary<int,ColumnMoveInfo>();
            ColumnMoveInfo columnMoveInfo;


            for (var column = 0; column < _fieldColumns; column++)
            {
                columnMoveInfo = new ColumnMoveInfo();
               
                for (var row = _fieldRows - 2; row != -1; row--)
                {
                    
                    
                    if (!_field[column, row].isEmpty)
                    {
                        var isElementMoved = false;
                        Vector2Int movedElement = Vector2Int.zero;
                        for (var movingRow = row; movingRow < _fieldRows - 1; movingRow++)
                        {
                            bool isMove = _field[column, movingRow + 1].isEmpty;
                            if (_field[column, movingRow + 1].isEmpty)

                            {
                                if (!isElementMoved)
                                {
                                    isElementMoved = true;
                                    movedElement = new Vector2Int(column, movingRow);
                                }

                                var currentCell = _field[column, movingRow];
                                var targetCell = _field[column, movingRow + 1];
                                targetCell.isEmpty = false;
                                targetCell.element = currentCell.element;
                                currentCell.isEmpty = true;
                            }
                            
                            if(!_field[column, movingRow + 1].isEmpty && !isMove  || movingRow == _fieldRows - 2)
                            {

                                if (isElementMoved)
                                {
                                    Debug.Log(!_field[column, movingRow + 1].isEmpty);
                                    Debug.Log(movingRow == _fieldRows - 2);
                                    columnMoveInfo.moveDistance = movingRow + 1 - movedElement.y;
                                    Debug.Log($"column {column} ==");
                                    Debug.Log($"movingRow {movingRow} ==");
                                    Debug.Log($"movedElement.y {movedElement.y} ==");
                                    Debug.Log($"columnMoveInfo.moveDistance {columnMoveInfo.moveDistance} ==");
                                    columnMoveInfo.oldElements.Add(movedElement);
                                }

                                break;
                            }
                        }
                        
                    }
                }

                if (columnMoveInfo.oldElements.Count > 0)
                {
                    columnMoveInfos.Add(column,columnMoveInfo);
                }
            }


            return columnMoveInfos;

        }

        public void AddNewElements(Dictionary<int,ColumnMoveInfo> columnMoveInfos )
        {
            for (int column = 0; column < _fieldColumns; column++)
            {
                bool isAddNewColumn = false;
                for (int row = _fieldColumns - 1; row != - 1; row--)
                {
                    if (_field[column, row].isEmpty)
                    {
                        _field[column, row].isEmpty = false;
                        _field[column, row].element = SelectRandomElement();
                        NewElementInfo newElement = new NewElementInfo();
                        newElement.element = _field[column, row].element;
                        newElement.coordinate = new Vector2Int(column, -row - 1);
                        if (!columnMoveInfos.ContainsKey(column))
                        {
                            isAddNewColumn = true;
                            ColumnMoveInfo columnMoveInfo = new ColumnMoveInfo();
                            columnMoveInfos.Add(column,columnMoveInfo);
                        }

                        columnMoveInfos[column].newElements.Add(newElement);
                    }
                }

                if (isAddNewColumn)
                {
                    columnMoveInfos[column].moveDistance = columnMoveInfos[column].newElements.Count;
                }
            

                
            }
        }

        private static HashSet<Vector2Int> FindMatchesByDirection(int iAmount, int jAmount,
            Func<int, int, Element> getElement, Func<int, int, Vector2Int> createCoordinate)
        {
            var matchedElementsCoordinates = new HashSet<Vector2Int>();
            var checkedElements = new HashSet<Vector2Int>();

            for (var i = 0; i < iAmount; i++)
            {
                var targetElement = getElement(0, i);
                var matches = 0;
                checkedElements.Clear();
                for (var j = 0; j < jAmount; j++)
                {
                    if (getElement(j, i) == targetElement)
                    {
                        matches++;
                        var elementCoordinate = createCoordinate(j, i);
                        checkedElements.Add(elementCoordinate);
                    }

                    if (getElement(j, i) != targetElement || j == jAmount - 1)
                    {
                        if (matches >= MIN_MATCHES)
                        {
                            foreach (var element in checkedElements)
                            {
                                matchedElementsCoordinates.Add(element);
                            }
                        }

                        targetElement = getElement(j, i);
                        matches = 1;
                        checkedElements.Clear();
                        checkedElements.Add(createCoordinate(j, i));
                    }
                }
            }

            return matchedElementsCoordinates;
        }

        private Element SelectSuitableElement(List<Element> columnPreviusElements, List<Element> rowPreviusElements)
        {
            var currentElement = Element.Yellow;

            for (var i = 0; i < 50; i++)
            {
                currentElement = SelectRandomElement();
                var isSuitableElementColumn = true;
                var isSuitableElementRow = true;
                if (columnPreviusElements.Count > 0)
                {
                    foreach (var previusElement in columnPreviusElements)
                    {
                        isSuitableElementColumn = currentElement != previusElement;
                    }
                }

                if (rowPreviusElements.Count > 0)
                {
                    foreach (var previusElement in rowPreviusElements)
                    {
                        isSuitableElementRow = currentElement != previusElement;
                    }
                }

                if (isSuitableElementColumn && isSuitableElementRow)
                {
                    break;
                }
            }


            columnPreviusElements.Clear();
            rowPreviusElements.Clear();
            return currentElement;
        }

        private Element SelectRandomElement()
        {
            return (Element) Random.Range(0, 4);
        }


        private void SendCellInfo(int column, int row, Element element, ChangeType changeType)
        {
            var eventArgs = new CellChangedArgs();
            eventArgs.column = column;
            eventArgs.row = row;
            eventArgs.changeType = changeType;
            eventArgs.element = element;
            CellChanged?.Invoke(eventArgs);
        }
    }
}