

namespace MonsterQuest
{
    public class FieldModel
    {
        private Cell[,] _field;
        private ILevelFieldData _levelFieldData;

        public FieldModel(ILevelFieldData levelFieldData)
        {
            _levelFieldData = levelFieldData;
            InitializeField();
        }
        
        private void InitializeField()
        {
            int rows = _levelFieldData.Field.GetLength(1);
            int columns = _levelFieldData.Field.GetLength(0);
            _field = new Cell[ columns, rows];
            for (int column = 0; column < columns; column++)
            {
                for (int row = 0; row < rows; row++)
                {
                    _field[column, row] = _levelFieldData.Field[column, row];
                }
            }
        }

    }
}