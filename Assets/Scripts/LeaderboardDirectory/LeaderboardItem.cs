namespace MonsterQuest
{
    public class LeaderboardItem
    {
        public int playerPosition;
        public string playerName;
        public int score;
        
        public LeaderboardItem(int playerPosition, string playerName, int score)
        {
            this.playerPosition = playerPosition;
            this.playerName = playerName;
            this.score = score;
        }
    }
}