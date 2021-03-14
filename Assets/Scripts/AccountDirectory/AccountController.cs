using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Zenject;

namespace MonsterQuest
{
    public class AccountController : MonoBehaviour
    {
        [Inject]
        private IRegisterView _registerView;
        [Inject]
        private IAccountInfoView _accountInfoView;

        

        [SerializeField]
        private RememberMeSettings _rememberMeSettings;

        

       
        private bool _isRememberMe;
        private void Start()
        {
            _registerView.Login += OnLogin;
        }

        private void OnLogin(LoginInfo info)
        {
            _isRememberMe = info.isRememberMe;
            if (info.type == LoginType.SignUp)
            {
                var request = new RegisterPlayFabUserRequest()
                    {DisplayName = info.userName, Password = info.password, Email = info.email, Username = info.userName };
                PlayFabClientAPI.RegisterPlayFabUser(request, OnSignUpSuccess, OnRegisterFailure);
            }
            else if(info.type == LoginType.SignIn)
            {
                LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest()
                    {Password = info.password, Email = info.email};
                PlayFabClientAPI.LoginWithEmailAddress(request, OnSignInSuccess, OnRegisterFailure);
                
            }
        }

        private void OnSignInSuccess(LoginResult result)
        {
            
            RememberMe();
            Debug.Log("Login");
        }

        private void OnRegisterFailure(PlayFabError obj)
        {
            Debug.LogError(obj.GenerateErrorReport());
        }

        private void OnSignUpSuccess(RegisterPlayFabUserResult result)
        {
            
            RememberMe();
            Debug.Log("register");
        }

        private void RememberMe()
        {
            if (_isRememberMe)
            {


                PlayerPrefs.SetInt(_rememberMeSettings.IsRememberMeKey, 1);
                PlayerPrefs.Save();
                string customId = Guid.NewGuid().ToString();
                PlayerPrefs.SetString(_rememberMeSettings.RememberMeIdKey, customId);
                PlayerPrefs.Save();
                LinkCustomIDRequest linkRequest = new LinkCustomIDRequest()
                {
                    CustomId = customId,
                    ForceLink = true,
                };
                PlayFabClientAPI.LinkCustomID(linkRequest, OnLinkSuccess, OnRegisterFailure);

                GetPlayerProfileRequest profileRequest = new GetPlayerProfileRequest()
                {
                    ProfileConstraints = new PlayerProfileViewConstraints()
                    {
                        ShowDisplayName = true,
                    }
                };
                PlayFabClientAPI.GetPlayerProfile(profileRequest, OnGetProfileSuccess, OnRegisterFailure);
            }
           
            
        }

        private void OnGetProfileSuccess(GetPlayerProfileResult result)
        {
            Debug.Log(result.PlayerProfile.DisplayName);
            _accountInfoView.Show(result.PlayerProfile.DisplayName);
        }

        private void OnLinkSuccess(LinkCustomIDResult obj)
        {
            Debug.Log("LinkAdded");
        }
    }
}