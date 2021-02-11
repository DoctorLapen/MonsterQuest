using System;

namespace MonsterQuest
{
    public class TurnsCounter : ITurnsCounter
    {
        public event Action<int> AmountChanged;
        public bool IsTurnsOver => _currentTurnsAmount == 0;
        public int Amount => _currentTurnsAmount;
        private int _currentTurnsAmount ;

        public TurnsCounter(int turnsForGame)
        {
            _currentTurnsAmount = turnsForGame;
            AmountChanged?.Invoke(_currentTurnsAmount);
        }

        public void СountTurn()
        {
            _currentTurnsAmount--;
            AmountChanged?.Invoke(_currentTurnsAmount);
        }

    }
}