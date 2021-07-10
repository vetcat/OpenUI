using Models.Player;
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
        }

        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<PlayerService>().AsSingle().NonLazy();
        }
    }
}