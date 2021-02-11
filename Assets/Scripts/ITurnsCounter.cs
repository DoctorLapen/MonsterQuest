using System;

namespace MonsterQuest
{
    public interface ITurnsCounter
    {
        bool IsTurnsOver { get; }
        int Amount { get; }
        void СountTurn();
        event Action<int> AmountChanged;
    }
}