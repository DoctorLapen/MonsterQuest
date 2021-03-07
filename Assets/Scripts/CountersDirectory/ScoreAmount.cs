using System;

namespace MonsterQuest
{
    [Serializable]
    public class ScoreAmount
    {
        public int score;

        public ScoreAmount()
        {
            
        }
        public ScoreAmount(int score)
        {
            this.score = score;
        }
    }
}