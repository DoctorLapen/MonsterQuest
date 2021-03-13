namespace MonsterQuest
{
    public interface ILeaderboardController
    {
        void SendScore(int score);
        void ShowLeaderboard();
    }
}