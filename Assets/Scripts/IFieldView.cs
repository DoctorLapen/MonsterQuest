namespace MonsterQuest
{
    public interface IFieldView
    {
        void SpawnBackgroundCell(int column,int row);
        void SpawnElement(int column, int row, Element element);
    }
}