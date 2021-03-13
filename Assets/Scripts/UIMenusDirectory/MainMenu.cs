using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterQuest
{
    public class MainMenu : Menu
    {
        [SerializeField]
        private RectTransform _leaderboardPane;

        
        public void StartGame()
        {
            SceneManager.LoadScene("Scenes/GameField");
        }

        public void OpenLeaderboard()
        {
            _leaderboardPane.gameObject.SetActive(true);
        }

        public void OpenPlayerAccount()
        {
            PreviousSceneHolder.SceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("Scenes/Account");
        }

    }
}