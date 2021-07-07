using System;
using Libs.OpenUI.ModalWindows;
using UniRx;
using Zenject;

namespace UiDevelopScene.Installers
{
    public class UiDevelopSceneInstaller : MonoInstaller
    {
        [Inject] private IModalWindowController _modalWindowController;
        public override void InstallBindings()
        {
            ShowInfoOkCancel();
        }

        private void ShowInfoOkCancel()
        {
            Observable.Timer(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    _modalWindowController.InfoOkCancel("test caption", "test description",
                        () => ShowAndHideWaitWindow(2), () => ShowAndHideWaitWindow(3));
                });
        }

        private void ShowAndHideWaitWindow(float timeProcess)
        {
            _modalWindowController.ShowWait(true,"Caption");
            Observable.Timer(TimeSpan.FromSeconds(timeProcess))
                .Subscribe(_ =>
                {
                    _modalWindowController.ShowWait(false);
                    //test stack modale window
                    _modalWindowController.InfoOk("Caption_1", "Description_1 - stack test");
                    _modalWindowController.InfoOk("Caption_2", "Description_2 - stack test");
                });
        }
    }
}