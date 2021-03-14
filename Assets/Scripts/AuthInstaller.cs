using UnityEngine;
using Zenject;

namespace MonsterQuest
{
    public class AuthInstaller : MonoInstaller
    {
        [SerializeField]
        private RememberMeFilePath _rememberMeFilePath;

        
        public override void InstallBindings()
        {
          
            Container.Bind<IRememberMeSaver>().To<RememberMeSaverXml>().AsSingle().WithArguments(_rememberMeFilePath.RememberMeFileName);
        }
    }
}