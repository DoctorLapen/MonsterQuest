using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterQuest
{
    public class AccountInfoView : MonoBehaviour, IAccountInfoView
    {
        [SerializeField]
        private Text _playerNameText;

        [SerializeField]
        private string _playerNameLabel;
        [SerializeField]
        private RememberMeSettings _rememberMeSettings;

        

        
        public void Show(string playerName)
        {
            gameObject.SetActive(true);
            _playerNameText.text = $"{_playerNameLabel}:\n{playerName}";
        }
        public void ChangePlayer()
        {
            gameObject.SetActive(false);
        }
    }
}