using UnityEngine;
using Zenject;

namespace MonsterQuest
{
    public class GameInstaller:MonoInstaller
    {
        [SerializeField]
        private LevelData _levelData;
        [SerializeField]
        private ElementsViewSettings _elementsSettings;

        

        public override void InstallBindings()
        {
            Container.Bind<ILeveldData>().FromInstance(_levelData).AsSingle();
            Container.Bind<IElementsViewSettings>().FromInstance(_elementsSettings).AsSingle();
            Container.Bind<IFieldModel>().To<FieldModel>().AsSingle();
        }
    }
}