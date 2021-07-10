using System;
using Libs.OpenUI;
using Libs.OpenUI.Localization;
using Libs.OpenUI.UiEffects;
using ProjectContext.Providers;
using SampleScene.UiViews.Views;
using UniRx;

namespace SampleScene.UiViews.Presenters
{
    public class UiTopCenterViewPresenter : UiPresenter<UiTopCenterView>
    {
        private readonly ITimeProvider _timeProvider;
        private readonly ILocalizationProvider _localization;
        private IDisposable _uiUpdater;
        
        public UiTopCenterViewPresenter(ITimeProvider timeProvider, ILocalizationProvider localization)
        {
            _timeProvider = timeProvider;
            _localization = localization;
        }

        public override void Initialize()
        {
            base.Initialize();

            OnShow
                .Subscribe(_ =>
                {
                    _uiUpdater = Observable.Interval(TimeSpan.FromSeconds(UiConstants.UiUpdateTime))
                        .Subscribe(_ => UpdateLocalTime());
                })
                .AddTo(Disposables);

            OnHide
                .Subscribe(_ => { _uiUpdater?.Dispose(); })
                .AddTo(Disposables);

            Show();
            UpdateLocalTime();
        }

        public override void ShowWithAnimation(Action complete = null)
        {
            View.ExpandAnimation(View.Body, EAnimationTarget.Down);
        }

        public override void HideWithAnimation(Action complete = null)
        {
            View.CollapseAnimation(View.Body, EAnimationTarget.Up);
        }

        private void UpdateLocalTime()
        {
            //localization template
            var time = _timeProvider.GetUtcTime.ToString("HH:mm:ss");
            var localizationTime = _localization.Get("TimeTemplate", time);
            View.TextLocalTime.text = localizationTime;
        }
    }
}