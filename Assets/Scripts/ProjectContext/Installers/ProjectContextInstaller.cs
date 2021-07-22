using Libs.OpenUI;
using Libs.OpenUI.Localization;
using Libs.OpenUI.Signals;
using Models.Player;
using ProjectContext.Providers;
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

            BindControllers();
            BindProviders();
            BindModels();
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
        
        private void BindControllers()
        {
            Container.BindInitializableExecutionOrder<LocalizationSetter>(-100);
            Container.BindInterfacesAndSelfTo<LocalizationSetter>().AsSingle();
            Container.BindInitializableExecutionOrder<LocalizationProvider>(-100);
            Container.BindInterfacesAndSelfTo<LocalizationProvider>().AsSingle().NonLazy();
        }

        private void BindProviders()
        {
            Container.BindInterfacesAndSelfTo<GameSettingsProvider>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TimeProvider>().AsSingle().NonLazy();
        }
        
        private void BindModels()
        {
            Container.BindInterfacesAndSelfTo<Player>().AsSingle().NonLazy();
        }

        //declaration of signals common to all scenes
        private void DeclareProjectSignals()
        {
            Container.DeclareSignal<SignalButtonClick>().OptionalSubscriber().RunSync();
            Container.DeclareSignal<SignalShowView>().OptionalSubscriber().RunSync();
            Container.DeclareSignal<SignalHideView>().OptionalSubscriber().RunSync();
            Container.DeclareSignal<SignalHideAllUi>().OptionalSubscriber().RunSync();
            Container.DeclareSignal<SignalRestoreHiddenUi>().OptionalSubscriber().RunSync();
        }
    }
}