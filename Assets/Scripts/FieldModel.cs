

using System;
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
            for (int column = 0; column < _fieldColumns ; column++)
            {
                for (int row = 0; row < _fieldRows; row++)
                {
                    Cell cell = _field[column, row];
                    cell.element = SelectRandomElement();
                    SendCellInfo(column, row,cell.element,ChangeType.Initialize);
                }
                
            }
        }

        private Element SelectRandomElement()
        {
            return (Element)Random.Range(0, 4);
           
        }

        private void SendCellInfo(int column, int row,Element element,ChangeType changeType)
        {
            Cell cell = _field[column, row];
            if (!cell.IsEmpty)
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
}