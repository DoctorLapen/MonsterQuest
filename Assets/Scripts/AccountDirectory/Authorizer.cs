using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace MonsterQuest
{
    public class Authorizer : MonoBehaviour
    {
        [SerializeField]
        private string _anonymousPrefsKey;

        private void Start()
        {
            string anonymousCustomId = String.Empty;
            bool anonymousKeyExist = PlayerPrefs.HasKey(_anonymousPrefsKey);
            if (anonymousKeyExist)
            { 
                anonymousCustomId = PlayerPrefs.GetString(_anonymousPrefsKey);
               CurrentAuth.auth = AuthType.Anonymous;
              
            }
            else if(!anonymousKeyExist)
            {
                CurrentAuth.auth = AuthType.Anonymous;
                anonymousCustomId = Guid.NewGuid().ToString();
                PlayerPrefs.SetString(_anonymousPrefsKey,anonymousCustomId);
                PlayerPrefs.Save();
            }
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
            {
                CustomId = anonymousCustomId,
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