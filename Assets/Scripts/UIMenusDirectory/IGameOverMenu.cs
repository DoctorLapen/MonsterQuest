namespace MonsterQuest
{
    public interface IGameOverMenu
    {
        void ShowMenu();
        void ChangeScore(int score,int record, bool isNewRecord);

        void Restart();
    }
}