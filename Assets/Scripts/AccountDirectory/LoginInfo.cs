namespace MonsterQuest
{
    public class LoginInfo
    {
        public string userName;
        public string email;
        public string password;
        public LoginType type;

    }

    public enum LoginType
    {
        SignUp,
        SignIn,
    }
}