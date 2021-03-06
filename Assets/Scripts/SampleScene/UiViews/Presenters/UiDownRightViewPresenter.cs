using System;
using Libs.OpenUI;
using Libs.OpenUI.UiEffects;
using SampleScene.UiViews.Views;
using UniRx;

namespace SampleScene.UiViews.Presenters
{
    public class UiDownRightViewPresenter : UiPresenter<UiDownRightView>
    {
        private readonly IUiSettingsViewPresenter _uiSettingsViewPresenter;

        public UiDownRightViewPresenter(IUiSettingsViewPresenter uiSettingsViewPresenter)
        {
            _uiSettingsViewPresenter = uiSettingsViewPresenter;
        }

        public override void Initialize()
        {
            base.Initialize();
            View.ButtonSettings.OnClickAsObservable()
                .Subscribe(_ => _uiSettingsViewPresenter.Open())
                .AddTo(Disposables);
        }

        public override void ShowWithAnimation(Action complete = null)
        {
            View.ExpandAnimation(View.Body, EAnimationTarget.Down);
        }

        public override void HideWithAnimation(Action complete = null)
        {
            View.CollapseAnimation(View.Body, EAnimationTarget.Up);
        }
    }
}