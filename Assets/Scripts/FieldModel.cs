

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

namespace MonsterQuest
{
    public class FieldModel : IFieldModel
    {
        public event Action<CellChangedArgs> CellChanged; 
        public event Action<ElementsReplacedArgs> ElementsReplaced; 
        private Cell[,] _field;
        private ILeveldData _levelData;
        private int _fieldColumns;
        private int _fieldRows;
        private const int MIN_MATCHES = 3;

        public FieldModel(ILeveldData levelData)
        {
            _levelData = levelData;
            _field = levelData.Field;
            _fieldColumns = levelData.Field.GetLength(0);
            _fieldRows = levelData.Field.GetLength(1);
        }

        public void InitializeField()
        {
            List<Element> columnPreviusElements = new List<Element>();
            List<Element> rowPreviusElements = new List<Element>();
            for (int column = 0; column < _fieldColumns ; column++)
            {
                for (int row = 0; row < _fieldRows; row++)
                {
                    Cell cell = _field[column, row];
                    if (!cell.IsExist)
                    {
                        
                        Element currentElement = Element.Yellow;
                        
                        if (column > 1 && row > 1)
                        { 
                            columnPreviusElements.Add(_field[column - 2,row].element);
                            columnPreviusElements.Add(_field[column - 1,row].element);
                            rowPreviusElements.Add(_field[column ,row - 2].element);
                            rowPreviusElements.Add(_field[column ,row - 1].element);
                            currentElement = SelectSuitableElement(columnPreviusElements,rowPreviusElements );
                        }
                        else if (column > 1 )
                        {
                            columnPreviusElements.Add(_field[column - 2,row].element);
                            columnPreviusElements.Add(_field[column - 1,row].element);
                            currentElement = SelectSuitableElement(columnPreviusElements,rowPreviusElements );
                        }
                        else if (row > 1)
                        {
                            rowPreviusElements.Add(_field[column ,row - 2].element);
                            rowPreviusElements.Add(_field[column ,row - 1].element);
                            currentElement = SelectSuitableElement(columnPreviusElements,rowPreviusElements );
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

        public bool IsElementInField(Vector2Int elementCoordinates)
        {
            bool isXInField = 0 <= elementCoordinates.x  &&  elementCoordinates.x < _fieldColumns;
            bool isYInField = 0 <= elementCoordinates.y  &&  elementCoordinates.y < _fieldRows;
            return isXInField && isYInField;
        }
        public HashSet<Vector2Int> FindMatchedElements(Vector2Int coordinateA,Vector2Int coordinateB)
        {
            Cell[,] _testField = new Cell[_fieldColumns, _fieldRows];
            Array.Copy(_field,_testField,_field.Length);
            Cell elementA = _testField[coordinateA.x, coordinateA.y];
            Cell elementB = _testField[coordinateB.x, coordinateB.y];
            _testField[coordinateA.x, coordinateA.y] = elementB  ;
            _testField[coordinateB.x, coordinateB.y] = elementA  ;
            var matchedHorizontalElements =
                FindMatchesByDirection(_fieldRows,_fieldColumns,(j,i)=> _testField[j,i].element,(j,i)=>new Vector2Int(j,i));
            var matchedVerticalElements =
                FindMatchesByDirection(_fieldColumns,_fieldRows,(j,i)=> _testField[i,j].element,(j,i)=>new Vector2Int(i,j));
            matchedHorizontalElements.UnionWith(matchedVerticalElements);
            return matchedHorizontalElements;
        }

        private static HashSet<Vector2Int> FindMatchesByDirection(int iAmount,int jAmount,Func<int,int,Element> getElement,Func<int,int,Vector2Int> createCoordinate)
        {
            HashSet<Vector2Int> matchedElementsCoordinates = new HashSet<Vector2Int>();
            HashSet<Vector2Int> checkedElements = new HashSet<Vector2Int>();

            for (int i = 0; i < iAmount; i++)
            {
                Element targetElement = getElement(0,i);
                int matches = 0;
                checkedElements.Clear();
                for (int j = 0; j < jAmount; j++)
                {
                    
                    if (getElement(j,i) == targetElement)
                    {
                        matches++;
                        Vector2Int elementCoordinate = createCoordinate(j, i);
                        checkedElements.Add(elementCoordinate);
                        
                    }
                    if (getElement(j,i) != targetElement || j == jAmount - 1)
                    {
                        if (matches >= MIN_MATCHES)
                        {
                            foreach (Vector2Int element in checkedElements)
                            {
                                matchedElementsCoordinates.Add(element);
                            }
                        }

                        targetElement = getElement(j,i);
                        matches = 1;
                        checkedElements.Clear();
                        checkedElements.Add(createCoordinate(j, i));
                    }
                }
            }

            return matchedElementsCoordinates;
        }

        public void ReplaceElements(Vector2Int coordinatesA,Vector2Int coordinatesB)
        {
            Cell elementA = _field[coordinatesA.x, coordinatesA.y];
            Cell elementB = _field[coordinatesB.x, coordinatesB.y];
            _field[coordinatesA.x, coordinatesA.y] = elementB ;
            _field[coordinatesB.x, coordinatesB.y] = elementA;
            ElementsReplacedArgs args = new ElementsReplacedArgs();
            args.elementA = coordinatesA;
            args.elementB = coordinatesB;
            ElementsReplaced?.Invoke(args);

        }

        public void DeleteElements(IEnumerable<Vector2Int> elementsCoordinates)
        {
            foreach (Vector2Int coordinate in elementsCoordinates)
            {
                Debug.Log(coordinate);
                _field[coordinate.x, coordinate.y].isEmpty = true;
                SendCellInfo(coordinate.x, coordinate.y, Element.Yellow, ChangeType.Delete);
            }
        }

        public void FillEmptyCells()
        {
            for (int row = _fieldRows - 2; row != - 1; row--)
            {
                for (int column = 0; column < _fieldColumns; column++)
                {
                    Debug.Log($"  Column- {column} row -{row}");
                    Debug.Log(_field[column, row].isEmpty);
                    if (!_field[column, row].isEmpty)
                    {
                        for (int movingRow = row ; movingRow < _fieldRows - 1; movingRow++)
                        {
                            Debug.Log(_field[column,movingRow + 1].isEmpty);
                            Debug.Log($" currentCell Column- {column} row -{movingRow}");
                            Debug.Log($"  targetCell Column- {column} row -{movingRow + 1}");
                            if (_field[column,movingRow + 1].isEmpty)
                            {
                                
                                Cell currentCell = _field[column, movingRow];
                                Cell targetCell = _field[column, movingRow + 1];
                                targetCell.isEmpty = false;
                                targetCell.element = currentCell.element;
                                currentCell.isEmpty = true;
                                SendCellInfo(column, movingRow,currentCell.element,ChangeType.MoveDown);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    
                }
            }
        }

        private Element SelectSuitableElement(List<Element> columnPreviusElements,List<Element> rowPreviusElements)
        {
            Element currentElement = Element.Yellow;

            for (int i = 0; i < 50; i++)
            {
                currentElement = SelectRandomElement();
                bool isSuitableElementColumn = true;
                bool isSuitableElementRow = true;
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
            return (Element)Random.Range(0, 4);
           
        }

       

        private void SendCellInfo(int column, int row,Element element,ChangeType changeType)
        {
            
            CellChangedArgs eventArgs = new CellChangedArgs();
            eventArgs.column = column;
            eventArgs.row = row;
            eventArgs.changeType = changeType;
            eventArgs.element = element;
            CellChanged?.Invoke(eventArgs);
            
        }
    }
}