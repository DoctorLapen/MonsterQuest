using System;

namespace MonsterQuest
{
    public interface IAccountInfoView
    {
        void Show(string playerName);
        
        event Action Logout;
    }
}