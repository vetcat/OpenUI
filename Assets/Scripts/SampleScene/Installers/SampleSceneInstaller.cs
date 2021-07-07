using UnityEngine;
using Zenject;

namespace SampleScene.Installers
{
    public class SampleSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("[SampleSceneInstaller] InstallBindings");
            
        }
    }
}