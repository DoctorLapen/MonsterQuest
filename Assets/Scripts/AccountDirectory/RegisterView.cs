using System;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterQuest
{
    public class RegisterView : MonoBehaviour, IRegisterView
    {
        public event Action<LoginInfo> Login;
        [SerializeField]
        private InputField _userNameField;

        [SerializeField]
        private InputField _emailField;
        
        [SerializeField]
        private InputField _passwordField;

        [SerializeField]
        private InputField _confirmPasswordField;
        [SerializeField]
        private Text _submitTextButton;

        [SerializeField]
        private Toggle _RememberMeToggle;

        
        
        
        
        private LoginType _mode;

        private void Awake()
        {
            SwitchToSignUp();

        }

        public void LoginAccount( )
        {
           
                if (_passwordField.text == _confirmPasswordField.text && _mode == LoginType.SignUp)
                {
                    Login?.Invoke(new LoginInfo()
                        {email = _emailField.text,
                            userName = _userNameField.text,
                            password = _passwordField.text,
                            type = _mode,
                            isRememberMe = _RememberMeToggle.isOn,
                            
                        });
                }
                else if (_mode == LoginType.SignIn)
                {
                    Login?.Invoke(new LoginInfo()
                    {
                        email = _emailField.text, password = _passwordField.text,type = _mode,
                        isRememberMe = _RememberMeToggle.isOn,
                    });
                }
            
            

        }

        public void SwitchToSignIn()
        {
            _userNameField.gameObject.SetActive(false);
            _confirmPasswordField.gameObject.SetActive(false);
            _mode = LoginType.SignIn;
            _submitTextButton.text = "Sign In";
        }
        public void SwitchToSignUp()
        {
            _userNameField.gameObject.SetActive(true);
            _confirmPasswordField.gameObject.SetActive(true);
            _mode = LoginType.SignUp;
            _submitTextButton.text = "Sign Up";
        }





    }
}