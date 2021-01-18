namespace MonsterQuest
{
    public class CellChangedArgs
    {
        public int column;
        public int row;
        public ChangeType changeType;
        public Element element;

    }

    public enum ChangeType
    {
        Initialize,
        InGame,
    }
}