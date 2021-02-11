namespace MonsterQuest
{
    public interface ILeveldData
    {
        Cell[,] Field { get; }
        public int TurnsForLevel { get; }
    }
}