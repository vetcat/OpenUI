using Zenject;

namespace UiDevelopScene.Installers
{
    public class UiDevelopSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UiDevelopSceneModalWindowShowController>().AsSingle().NonLazy();
        }
    }
}