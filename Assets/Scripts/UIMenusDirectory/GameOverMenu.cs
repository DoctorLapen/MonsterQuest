using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MonsterQuest
{
    public class GameOverMenu : Menu, IGameOverMenu
    {
        [SerializeField]
        private RectTransform _menuPane;


        [SerializeField]
        private Text _scoreText;
        [SerializeField]
        private Text _recordText;

        [SerializeField]
        private Text _newRecordText;
        

        [SerializeField]
        private string _scoreLabel;
        [SerializeField]
        private string _recordLabel;



        public void ShowMenu()
        {
            _menuPane.gameObject.SetActive(true);
        }

        public void ChangeScore(int score, int record, bool isNewRecord)
        {
            _scoreText.text = $"{_scoreLabel} {score}";
            _recordText.text = $"{_recordLabel} {score}";
            if (isNewRecord)
            {
                _newRecordText.gameObject.SetActive(true);
            }
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}