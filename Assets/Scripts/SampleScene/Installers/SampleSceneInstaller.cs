using ProjectContext.Models.Player;
using SampleScene.Models.Character;
using SampleScene.Providers;
using UnityEngine;
using Zenject;

namespace SampleScene.Installers
{
    public class SampleSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("[SampleSceneInstaller] InstallBindings");

            BindProviders();
            BindServices();
            BindControllers();
        }

        private void BindProviders()
        {
            Container.BindInterfacesAndSelfTo<CameraProvider>().AsSingle().NonLazy();
        }
        
        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<PlayerService>().AsSingle().NonLazy();
        }

        private void BindControllers()
        {
            Container.BindInterfacesAndSelfTo<CharacterViewController>().AsSingle().NonLazy();
        }
    }
}