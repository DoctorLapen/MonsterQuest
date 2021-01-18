

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MonsterQuest
{
    public class FieldModel : IFieldModel
    {
        public event Action<CellChangedArgs> CellChanged; 
        private Cell[,] _field;
        private ILeveldData _levelData;
        private int _fieldColumns;
        private int _fieldRows;

        public FieldModel(ILeveldData levelData)
        {
            _levelData = levelData;
            _field = levelData.Field;
            _fieldColumns = levelData.Field.GetLength(0);
            _fieldRows = levelData.Field.GetLength(1);
        }

        public void InitializeField()
        {
            List<Element> previusElements = new List<Element>();
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
                            previusElements.Add(_field[column - 2,row].element);
                            previusElements.Add(_field[column - 1,row].element);
                            previusElements.Add(_field[column ,row - 2].element);
                            previusElements.Add(_field[column ,row - 1].element);
                            currentElement = SelectSuitableElement(previusElements);
                        }
                        else if (column > 1 )
                        {
                            previusElements.Add(_field[column - 2,row].element);
                            previusElements.Add(_field[column - 1,row].element);
                            currentElement = SelectSuitableElement(previusElements);
                        }
                        else if (row > 1)
                        {
                            previusElements.Add(_field[column ,row - 2].element);
                            previusElements.Add(_field[column ,row - 1].element);
                            currentElement = SelectSuitableElement(previusElements);
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

        private Element SelectSuitableElement(List<Element> previusElements)
        {
            Element currentElement = Element.Yellow;
            for (int i = 0; i < 15; i++)
            {
                currentElement = SelectRandomElement();
                bool isSuitableElement = false;
                foreach (var previusElement in previusElements)
                {
                    isSuitableElement = currentElement != previusElement;
                }

                if (isSuitableElement)
                {
                    break;
                }
            }
            previusElements.Clear();
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