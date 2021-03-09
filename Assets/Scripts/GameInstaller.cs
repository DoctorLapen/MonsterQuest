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

        [SerializeField]
        private string _saveXmlFileName;

        

        

        public override void InstallBindings()
        {
            Container.Bind<ILeveldData>().FromInstance(_levelData).AsSingle();
            Container.Bind<IElementsViewSettings>().FromInstance(_elementsSettings).AsSingle();
            Container.Bind<IFieldModel>().To<FieldModel>().AsSingle();
            Container.Bind<ITurnsCounter>().To<TurnsCounter>().AsSingle().WithArguments(_levelData.TurnsForLevel);
            Container.Bind<IScoreCounter>().To<ScoreCounter>().AsSingle().WithArguments(_levelData.OneElementCost);
            Container.Bind<IScoreSaver>().To<ScoreSaverXml>().AsSingle().WithArguments(_saveXmlFileName);
        }
    }
}