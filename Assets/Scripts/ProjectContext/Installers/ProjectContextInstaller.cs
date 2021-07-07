using Libs.OpenUI;
using Libs.OpenUI.Signals;
using UnityEngine;
using Zenject;

namespace ProjectContext.Installers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("[ProjectContextInstaller] InstallBindings");

            SetProjectSettings();
            SignalBusInstaller.Install(Container);
            DeclareProjectSignals();
            
            Container.Bind<AsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }

        private void SetProjectSettings()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
            Debug.unityLogger.logEnabled = false;
#endif
        }
        
        //declaration of signals common to all scenes
        private void DeclareProjectSignals()
        {
            Container.DeclareSignal<SignalButtonClick>().RunSync();
            Container.DeclareSignal<SignalShowView>().RunSync();
            Container.DeclareSignal<SignalHideAllUi>().RunSync();
            Container.DeclareSignal<SignalRestoreHiddenUi>().RunSync();
        }
    }
}