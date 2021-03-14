using System;

namespace MonsterQuest
{
    [Serializable]
    public class RememberMeInfo
    {
        
        public bool isRememberMe;
        public string customId;
        public string playerName = String.Empty;

        public RememberMeInfo()
        {
        }
        public RememberMeInfo(bool isRememberMe, string customId)
        {
            this.isRememberMe = isRememberMe;
            this.customId = customId;
        }
    }
}