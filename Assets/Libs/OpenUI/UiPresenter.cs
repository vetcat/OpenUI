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
		private readonly ReactiveCommand<UiPresenter> _onShow = new();
		public IObservable<UiPresenter> OnShow => _onShow;

		private readonly ReactiveCommand<UiPresenter> _onHide = new();
		public IObservable<UiPresenter> OnHide => _onHide;

		public virtual void Initialize()
		{
		}

		public virtual void Dispose()
		{
		}

		public virtual void Show() => _onShow.Execute(this);

		public virtual void Hide() => _onHide.Execute(this);

		public abstract bool IsShow();
	}

	public abstract class UiPresenter<T> : UiPresenter, IUiPresenter
		where T : UiView
	{
		protected readonly CompositeDisposable Disposables = new();

		[Inject] private T _view;

		[Inject] private ILocalizationProvider _localization;
		[Inject] private ILocalizationSetter _localizationSetter;
		[Inject] private SignalBus _signalBus;

		public override void Initialize()
		{
			LocalizableInit();
			_view.AddSignalClick(_signalBus);
		}

		public T View => _view;
		public UiView GetUiView => _view;

		public void Lock(bool value)
		{
			foreach (var button in _view.GetComponentsInChildren<Button>())
				button.enabled = !value;
		}

		public override void Show()
		{
			View.Show();
			base.Show();
			_signalBus.Fire(new SignalShowView(_view));
		}

		public override void Hide()
		{
			View.Hide();
			base.Hide();
			_signalBus.Fire(new SignalHideView(_view));
		}

		public override bool IsShow() => View != null && View.IsShow();

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

		protected void LocalizableInit()
		{
			_localizationSetter.InitLocalizable(GetUiView);
		}

		public ILocalizationProvider Localization => _localization;
	}
}