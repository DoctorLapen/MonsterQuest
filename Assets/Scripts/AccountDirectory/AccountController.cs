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
            var request = new RegisterPlayFabUserRequest(){DisplayName = info.userName,Password =info.password,Email = info.email};
            PlayFabClientAPI.RegisterPlayFabUser(request,OnRegisterSuccess,OnRegisterFailure);
        }

        private void OnRegisterFailure(PlayFabError obj)
        {
            Debug.LogError(obj.GenerateErrorReport());
        }

        private void OnRegisterSuccess(RegisterPlayFabUserResult result)
        {
            Debug.Log("register");
        }
    }
}