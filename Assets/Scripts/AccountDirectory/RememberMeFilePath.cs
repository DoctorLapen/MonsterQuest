using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(fileName = "RememberMeSettings", menuName = "MonsterQuest/RememberMeSettings", order = 4)]
    public class RememberMeFilePath : ScriptableObject
    {
        [SerializeField]
        private string _rememberMeFileName;

        public string RememberMeFileName => _rememberMeFileName;
        
        
    }
}