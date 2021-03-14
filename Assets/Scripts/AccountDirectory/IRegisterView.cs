using System;

namespace MonsterQuest
{
    public interface IRegisterView
    {
        event Action<LoginInfo> Login;

        void SetErrorText(string text);
    }
}