using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(fileName = "RememberMeSettings", menuName = "MonsterQuest/RememberMeSettings", order = 4)]
    public class RememberMeSettings : ScriptableObject
    {
        [SerializeField]
        private string _rememberMeIdKey;

        public string RememberMeIdKey => _rememberMeIdKey;
        [SerializeField]
        private string _isRememberMeKey;
        public string IsRememberMeKey => _isRememberMeKey;
        
    }
}