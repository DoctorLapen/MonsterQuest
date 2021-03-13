using UnityEngine;
using UnityEngine.UI;

namespace MonsterQuest
{
    public class LeaderboardItemView : MonoBehaviour
    {
        [SerializeField]
        private Text _playerPositionText;
        [SerializeField]
        private Text _playerNameText;
        [SerializeField]
        private Text _playerScoreText;

        
        public void ShowTitles(LeaderboardItem item)
        {
            _playerPositionText.text = item.playerPosition.ToString() + ".";
            _playerNameText.text = item.playerName;
            _playerScoreText.text = item.score.ToString();
        }
    }
}