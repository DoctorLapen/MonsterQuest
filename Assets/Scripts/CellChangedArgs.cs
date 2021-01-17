namespace MonsterQuest
{
    public class CellChangedArgs
    {
        public int column;
        public int row;
        public ChangeType changeType;

    }

    public enum ChangeType
    {
        Initialize,
        InGame,
    }
}