using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Zenject;

namespace MonsterQuest
{
    public class Authorizer : MonoBehaviour
    {
        [Inject]
        private IRememberMeSaver _rememberMeSaver;
        [SerializeField]
        private string _anonymousPrefsKey;
        

        private void Start()
        {
            string customId = String.Empty;
            bool anonymousKeyExist = PlayerPrefs.HasKey(_anonymousPrefsKey);
            RememberMeInfo rememberMeInfo = _rememberMeSaver.Load();
            if (rememberMeInfo.isRememberMe)
            {
                customId = rememberMeInfo.customId;
                CurrentAuth.type = AuthType.Login;
                CurrentAuth.playerDisplayName = rememberMeInfo.playerName;
            }

            else if (anonymousKeyExist)
            { 
                customId = PlayerPrefs.GetString(_anonymousPrefsKey);
               CurrentAuth.type = AuthType.Anonymous;
              
            }
            else if(!anonymousKeyExist)
            {
                CurrentAuth.type = AuthType.Anonymous;
                customId = Guid.NewGuid().ToString();
                PlayerPrefs.SetString(_anonymousPrefsKey,customId);
                PlayerPrefs.Save();
            }
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
            {
                CustomId = customId,
                CreateAccount = true,
            };
            PlayFabClientAPI.LoginWithCustomID(request,OnLoginSuccess,OnLoginFailure);
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.Log(error.Error);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            Debug.Log("Login");
        }
    }
}