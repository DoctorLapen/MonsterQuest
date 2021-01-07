

namespace MonsterQuest
{
    public class FieldModel
    {
        private Cell[,] _field;
        private ILeveldData _leveldData;

        public FieldModel(ILeveldData leveldData)
        {
            _leveldData = leveldData;
            InitializeField();
        }
        
        private void InitializeField()
        {
            int rows = _leveldData.Field.GetLength(1);
            int columns = _leveldData.Field.GetLength(0);
            _field = new Cell[ columns, rows];
            for (int column = 0; column < columns; column++)
            {
                for (int row = 0; row < rows; row++)
                {
                    _field[column, row] = _leveldData.Field[column, row];
                }
            }
        }

    }
}