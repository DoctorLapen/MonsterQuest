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

        [Inject]
        private IRememberMeSaver _rememberMeSaver;
        
       
        [SerializeField]
        private string _anonymousPrefsKey;

        

       
        private bool _isRememberMe;
        private RememberMeInfo _rememberMeInfo;
        private void Start()
        {
            _registerView.Login += OnLogin;
            _accountInfoView.Logout += OnLogout;
            if (CurrentAuth.type == AuthType.Login)
            {
                _accountInfoView.Show(CurrentAuth.playerDisplayName);
            }
        }

        private void OnLogin(LoginInfo info)
        {
            _isRememberMe = info.isRememberMe;
            if (info.type == LoginType.SignUp)
            {
                var request = new RegisterPlayFabUserRequest()
                    {DisplayName = info.userName, Password = info.password, Email = info.email, Username = info.userName };
                PlayFabClientAPI.RegisterPlayFabUser(request, OnSignUpSuccess, OnAuthFailure);
            }
            else if(info.type == LoginType.SignIn)
            {
                LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest()
                    {Password = info.password, Email = info.email};
                PlayFabClientAPI.LoginWithEmailAddress(request, OnSignInSuccess, OnAuthFailure);
                
            }
        }

        private void OnLogout()
        {
            CurrentAuth.type = AuthType.Anonymous;
            _rememberMeSaver.Delete();
            string customId = PlayerPrefs.GetString(_anonymousPrefsKey);
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
            {
                CustomId = customId,
                CreateAccount = true,
            };
            PlayFabClientAPI.LoginWithCustomID(request,OnAnonymousSuccess,OnAuthFailure);
            
        }

        private void OnAnonymousSuccess(LoginResult obj)
        {
            Debug.Log("Anonymous");
        }

        private void OnSignInSuccess(LoginResult result)
        {
            
            OnAuthSuccess();
            Debug.Log("Login");
        }

        private void OnAuthFailure(PlayFabError obj)
        {
            Debug.LogError(obj.GenerateErrorReport());
        }

        private void OnSignUpSuccess(RegisterPlayFabUserResult result)
        {
            
            OnAuthSuccess();
            Debug.Log("register");
        }

        private void OnAuthSuccess()
        {
            CurrentAuth.type = AuthType.Login;
            
            if (_isRememberMe)
            {

                _isRememberMe = false;
                
                string customId = Guid.NewGuid().ToString();
                _rememberMeInfo = new RememberMeInfo(true, customId);
                LinkCustomIDRequest linkRequest = new LinkCustomIDRequest()
                {
                    CustomId = customId,
                    ForceLink = true,
                };
                PlayFabClientAPI.LinkCustomID(linkRequest, OnLinkSuccess, OnAuthFailure);

               
            }
            GetPlayerProfileRequest profileRequest = new GetPlayerProfileRequest()
            {
                ProfileConstraints = new PlayerProfileViewConstraints()
                {
                    ShowDisplayName = true,
                }
            };
            PlayFabClientAPI.GetPlayerProfile(profileRequest, OnGetProfileSuccess, OnAuthFailure);
            
        }

        private void OnGetProfileSuccess(GetPlayerProfileResult result)
        {
            Debug.Log(result.PlayerProfile.DisplayName);
            _accountInfoView.Show(result.PlayerProfile.DisplayName);
            CurrentAuth.playerDisplayName = result.PlayerProfile.DisplayName;
        }

        private void OnLinkSuccess(LinkCustomIDResult obj)
        {
            _rememberMeInfo.playerName = CurrentAuth.playerDisplayName;
            _rememberMeSaver.Save(_rememberMeInfo);
            
            Debug.Log("LinkAdded");
        }
    }
}