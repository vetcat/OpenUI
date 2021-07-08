using System;
using System.Collections.Generic;
using Libs.OpenUI.Signals;
using UniRx;
using Zenject;

namespace Libs.OpenUI
{
    public class WindowsController : IWindowsController, IInitializable, IDisposable
    {
        private readonly List<IUiPresenter> _uiControllers;
        private readonly SignalBus _signalBus;
        private readonly List<IUiPresenter> _heap;
        readonly CompositeDisposable _disposable = new CompositeDisposable();

        public WindowsController(List<IUiPresenter> uiControllers, SignalBus signalBus)
        {
            _uiControllers = uiControllers;
            _signalBus = signalBus;
            _heap = new List<IUiPresenter>(uiControllers.Count);
        }
        
        public void Initialize()
        {
            _signalBus.GetStream<SignalHideAllUi>()
                .Subscribe(data => HideAll(data.UiPresenter))
                .AddTo(_disposable);
            
            _signalBus.GetStream<SignalRestoreHiddenUi>()
                .Subscribe(_ => RestoreHidden())
                .AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
        
        public void HideAll(IUiPresenter excludeUiPresenter)
        {
            foreach (var controller in _uiControllers)
            {
                if (controller.IsShow() && controller != excludeUiPresenter)
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