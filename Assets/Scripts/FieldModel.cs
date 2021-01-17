

using System;
using UnityEngine;

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
                    SendCellInfo(column, row,ChangeType.Initialize);
                }
                
            }
        }

        private void SendCellInfo(int column, int row,ChangeType changeType)
        {
            Cell cell = _field[column, row];
            if (!cell.IsEmpty)
            {
                CellChangedArgs eventArgs = new CellChangedArgs();
                eventArgs.column = column;
                eventArgs.row = row;
                eventArgs.changeType = changeType;
                CellChanged?.Invoke(eventArgs);
            }
        }
    }
}