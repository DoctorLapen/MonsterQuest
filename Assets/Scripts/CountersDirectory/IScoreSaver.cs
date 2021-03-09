namespace MonsterQuest
{
    public interface IScoreSaver
    {
        void Save(ScoreAmount score);
        ScoreAmount Load();
    }
}