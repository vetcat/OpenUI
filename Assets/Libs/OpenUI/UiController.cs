using System;
using Libs.OpenUI.Localization;
using Libs.OpenUI.Signals;
using Libs.OpenUI.UiEffects;
using UniRx;
using UnityEngine.UI;
using Zenject;

namespace Libs.OpenUI
{
    public interface IUiController
    {
        IObservable<UiController> OnShow { get; }
        IObservable<UiController> OnHide { get; }
        void Show();
        void ShowWithAnimation(Action complete = null);
        void Hide();
        void HideWithAnimation(Action complete = null);
        bool IsShow();
        void Lock(bool value);
        UiView GetUiView { get; }
    }

    public abstract class UiController : IInitializable, IDisposable
    {
        public virtual void Initialize()
        {
        }

        public virtual void Dispose()
        {
        }
    }

    public abstract class UiController<T> : UiController, IUiController
        where T : UiView
    {
        protected UiController()
        {
            _onShow = new Subject<UiController>();
            _onHide = new Subject<UiController>();
        }

        public IObservable<UiController> OnShow => _onShow;
        private readonly Subject<UiController> _onShow;
        public IObservable<UiController> OnHide => _onHide;
        private readonly Subject<UiController> _onHide;
        
        protected readonly CompositeDisposable Disposables = new CompositeDisposable();

        [Inject] private T _view;

        [Inject] private ILocalizationProvider _localization;
        [Inject] private ILocalizationSetter _localizationSetter;
        [Inject] private SignalBus _signalBus;
        
        public override void Initialize()
        {
            if (_view is ILocalizable)
                LocalizableInit((ILocalizable) _view);

            _view.AddSignalClick(_signalBus);
        }

        public T View => _view;
        public UiView GetUiView => _view;
        
        public void Lock(bool value)
        {
            foreach (var button in _view.GetComponentsInChildren<Button>())
                button.enabled = !value;
        }

        public virtual void Show()
        {
            View.Show();
            _onShow.OnNext(this);
            _signalBus.Fire(new SignalShowView(_view));
        }

        public virtual void Hide()
        {
            View.Hide();
            _onHide.OnNext(this);
        }

        public bool IsShow()
        {
            return View.IsShow();
        }

        public virtual void HideWithAnimation(Action complete = null)
        {
            Hide();
            complete?.Invoke();
        }
        
        public virtual void ShowWithAnimation(Action complete = null)
        {
            Show();
            complete?.Invoke();
        }

        public override void Dispose()
        {
            Disposables?.Dispose();
        }

        protected string Translate(string key, params object[] obj)
        {
            return _localization.Get(key, obj);
        }

        protected void LocalizableInit(ILocalizable localizable)
        {
            _localizationSetter.InitLocalizable(localizable);
        }
        
        public ILocalizationProvider Localization => _localization;
    }
}