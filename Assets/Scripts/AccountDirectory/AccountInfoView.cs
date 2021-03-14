using System;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterQuest
{
    public class AccountInfoView : MonoBehaviour, IAccountInfoView
    {
        public event Action Logout;
        [SerializeField]
        private Text _playerNameText;

        [SerializeField]
        private string _playerNameLabel;
       

        

        
        public void Show(string playerName)
        {
            gameObject.SetActive(true);
            _playerNameText.text = $"{_playerNameLabel}:\n{playerName}";
        }
        public void ExitAccount()
        {
            gameObject.SetActive(false);
            Logout?.Invoke();
        }
    }
}