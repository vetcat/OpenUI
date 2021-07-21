using System;
using Libs.OpenUI.Localization;
using Libs.OpenUI.Signals;
using Libs.OpenUI.UiEffects;
using UniRx;
using UnityEngine.UI;
using Zenject;

namespace Libs.OpenUI
{
    public interface IUiPresenter
    {
        IObservable<UiPresenter> OnShow { get; }
        IObservable<UiPresenter> OnHide { get; }
        void Show();
        void ShowWithAnimation(Action complete = null);
        void Hide();
        void HideWithAnimation(Action complete = null);
        bool IsShow();
        void Lock(bool value);
        UiView GetUiView { get; }
    }

    public abstract class UiPresenter : IInitializable, IDisposable
    {
        public virtual void Initialize()
        {
        }

        public virtual void Dispose()
        {
        }
    }

    public abstract class UiPresenter<T> : UiPresenter, IUiPresenter
        where T : UiView
    {
        protected UiPresenter()
        {
            _onShow = new Subject<UiPresenter>();
            _onHide = new Subject<UiPresenter>();
        }

        public IObservable<UiPresenter> OnShow => _onShow;
        private readonly Subject<UiPresenter> _onShow;
        public IObservable<UiPresenter> OnHide => _onHide;
        private readonly Subject<UiPresenter> _onHide;
        
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
            _signalBus.Fire(new SignalHideView(_view));
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