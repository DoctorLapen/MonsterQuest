using System;

namespace MonsterQuest
{
    public interface IRegisterView
    {
        event Action<LoginInfo> Login;
        
    }
}