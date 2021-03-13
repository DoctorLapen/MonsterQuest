using System;

namespace MonsterQuest
{
    public interface ILeaderboardView
    {
        void Clear();
        void AddItem(LeaderboardItem item);
        event Action UpdateBoard;
    }
}