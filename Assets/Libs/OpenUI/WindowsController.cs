using System;
using System.Collections.Generic;
using Libs.OpenUI.Signals;
using UniRx;
using Zenject;

namespace Libs.OpenUI
{
    public class WindowsController : IWindowsController, IInitializable, IDisposable
    {
        private readonly List<IUiController> _uiControllers;
        private readonly SignalBus _signalBus;
        private readonly List<IUiController> _heap;
        readonly CompositeDisposable _disposable = new CompositeDisposable();

        public WindowsController(List<IUiController> uiControllers, SignalBus signalBus)
        {
            _uiControllers = uiControllers;
            _signalBus = signalBus;
            _heap = new List<IUiController>(uiControllers.Count);
        }
        
        public void Initialize()
        {
            _signalBus.GetStream<SignalHideAllUi>()
                .Subscribe(data => HideAll(data.UiController))
                .AddTo(_disposable);
            
            _signalBus.GetStream<SignalRestoreHiddenUi>()
                .Subscribe(_ => RestoreHidden())
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
        
        public void HideAll(IUiController excludeUiController)
        {
            foreach (var controller in _uiControllers)
            {
                if (controller.IsShow() && controller != excludeUiController)
                {
                    _heap.Add(controller);
                    controller.Hide();
                }
            }
        }

        public void RestoreHidden()
        {
            foreach (var controller in _heap)
            {
                controller.Show();
            }
            
            _heap.Clear();
        }
    }
}