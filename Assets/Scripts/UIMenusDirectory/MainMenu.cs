using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterQuest
{
    public class MainMenu : Menu
    {
        public void StartGame()
        {
            SceneManager.LoadScene("Scenes/GameField");
        }

    }
}