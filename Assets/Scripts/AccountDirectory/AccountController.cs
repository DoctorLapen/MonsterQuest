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
        private void Start()
        {
            _registerView.Login += OnLogin;
        }

        private void OnLogin(LoginInfo info)
        {
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
            Debug.Log("Login");
        }

        private void OnRegisterFailure(PlayFabError obj)
        {
            Debug.LogError(obj.GenerateErrorReport());
        }

        private void OnSignUpSuccess(RegisterPlayFabUserResult result)
        {
            Debug.Log("register");
        }
    }
}