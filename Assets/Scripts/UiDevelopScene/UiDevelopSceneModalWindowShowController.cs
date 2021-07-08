using System;
using Libs.OpenUI.Localization;
using Libs.OpenUI.ModalWindows;
using UniRx;
using Zenject;

namespace UiDevelopScene
{
    //example of using modal windows 
    public class UiDevelopSceneModalWindowShowController : IInitializable
    {
        private readonly IModalWindowController _modalWindowController;
        private readonly ILocalizationProvider _localization;
        
        private string _caption;
        private string _description;

        public UiDevelopSceneModalWindowShowController(IModalWindowController modalWindowController, ILocalizationProvider localization)
        {
            _modalWindowController = modalWindowController;
            _localization = localization;
        }

        public void Initialize()
        {
            _caption = _localization.Get("TestCaption");
            _description = _localization.Get("TestDescription");
            ShowInfoOkCancel();
        }
        
        private void ShowInfoOkCancel()
        {
            _modalWindowController.InfoOkCancel(_caption, _description,
                () => ShowAndHideWaitWindow(2), () => ShowAndHideWaitWindow(3));
        }

        private void ShowAndHideWaitWindow(float timeProcess)
        {
            _modalWindowController.ShowWait(true, _caption);
            Observable.Timer(TimeSpan.FromSeconds(timeProcess))
                .Subscribe(_ =>
                {
                    _modalWindowController.ShowWait(false);
                    //test stack modale window
                    _modalWindowController.InfoOk($"{_caption}_{1}", $"{_description}_{1}");
                    _modalWindowController.InfoOk($"{_caption}_{2}", $"{_description}_{2}");
                });
        }
    }
}
