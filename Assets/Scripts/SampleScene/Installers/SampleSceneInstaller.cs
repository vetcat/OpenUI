using ProjectContext.Models.Player;
using SampleScene.Models.Character;
using UnityEngine;
using Zenject;

namespace SampleScene.Installers
{
    public class SampleSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("[SampleSceneInstaller] InstallBindings");

            BindServices();
            BindControllers();
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