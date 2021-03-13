using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(fileName = " LeaderboardPlayFabSettings", menuName = "MonsterQuest/LeaderboardPlayFabSettings ", order = 3)]
    public class LeaderboardPlayFabSettings : ScriptableObject
    {
        [SerializeField]
        private string _statisticName;

        public string StatisticName
        {
            get { return this._statisticName; }
        }

        [SerializeField]
        private int  _startPosition;

        public int  StartPosition
        {
            get { return this. _startPosition; }
        }

        [SerializeField]
        private int _maxResultCount;

        public int MaxResultCount
        {
            get { return this._maxResultCount; }
        }

        

        

        

        
    }
}