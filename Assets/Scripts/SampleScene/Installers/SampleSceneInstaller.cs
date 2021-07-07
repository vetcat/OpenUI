using Libs.OpenUI.ModalWindows;
using UnityEngine;
using Zenject;

namespace SampleScene.Installers
{
    public class SampleSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("[SampleSceneInstaller] InstallBindings");
            
            //todo temp
            var modalWindowsController = Container.Resolve<ModalWindowController>();
            modalWindowsController.InfoOkCancel("test caption", "test description", null, null);
        }
    }
}