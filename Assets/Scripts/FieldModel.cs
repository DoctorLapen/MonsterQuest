

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
                    if (!cell.IsEmpty)
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
            var matchedHorizontalElements = FindMatchesByDirection(_fieldRows,_fieldColumns,(j,i)=> _testField[j,i].element);
            var matchedVerticalElements = FindMatchesByDirection(_fieldColumns,_fieldRows,(j,i)=> _testField[i,j].element);
            matchedHorizontalElements.UnionWith(matchedVerticalElements);
            return matchedHorizontalElements;
        }

        private static HashSet<Vector2Int> FindMatchesByDirection(int iAmount,int jAmount,Func<int,int,Element> getElement)
        {
            HashSet<Vector2Int> matchedElementsCoordinates = new HashSet<Vector2Int>();
            HashSet<Vector2Int> checkedElements = new HashSet<Vector2Int>();

            for (int i = 0; i < iAmount; i++)
            {
                Element targetElement = getElement(0,i);
                int matches = 0;
                for (int j = 0; j < jAmount; j++)
                {
                    if (getElement(j,i) == targetElement)
                    {
                        matches++;
                        Vector2Int elementCoordinate = new Vector2Int(j, i);
                        checkedElements.Add(elementCoordinate);
                    }
                    else if (getElement(j,i) != targetElement || j == jAmount - 1)
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
            Debug.Log(currentElement);
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