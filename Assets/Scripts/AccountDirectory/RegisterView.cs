using System;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterQuest
{
    public class RegisterView : MonoBehaviour, IRegisterView
    {
        public event Action<LoginInfo> Login;
        [SerializeField]
        private InputField _SignUpUserNameField;

        [SerializeField]
        private InputField _emailField;
        [SerializeField]
        private InputField _passwordField;

        [SerializeField]
        private InputField _confirmPasswordField;
        
        public void SignUp( )
        {
            if (_passwordField.text == _confirmPasswordField.text)
            {
                Login?.Invoke(
                    new LoginInfo(){email = _emailField.text,userName = _SignUpUserNameField.text ,password = _passwordField.text});
            }
        }
        

        

        
    }
}