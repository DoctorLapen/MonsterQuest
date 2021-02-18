using System.Collections.Generic;

namespace MonsterQuest
{
    public interface IFieldView
    {
        void SpawnBackgroundCell(int column,int row);
        void SpawnElement(int column, int row, Element element);
        void ReplaceVisualElements (int columnA,int rowA,int columnB,int rowB);
        void DeleteElement(int column, int row);
        void MoveDownElements(List<ColumnMoveInfo> columnMoveInfos);
    }
}