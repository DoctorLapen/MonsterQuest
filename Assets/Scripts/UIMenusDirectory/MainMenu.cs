
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace MonsterQuest
{
    public class MainMenu : Menu
    {
        [Inject]
        private ILeaderboardController _leaderboardController;
           
        [SerializeField]
        private RectTransform _leaderboardPane;

        
        public void StartGame()
        {
            SceneManager.LoadScene("Scenes/GameField");
        }

        public void OpenLeaderboard()
        {
            _leaderboardPane.gameObject.SetActive(true);
            _leaderboardController.ShowLeaderboard();
        }

        public void OpenPlayerAccount()
        {
            PreviousSceneHolder.SceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("Scenes/Account");
        }

    }
}