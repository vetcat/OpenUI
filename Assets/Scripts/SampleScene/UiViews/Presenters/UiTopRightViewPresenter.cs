using Libs.OpenUI;
using SampleScene.UiViews.Views;
using UniRx;

namespace SampleScene.UiViews.Presenters
{
    public class UiTopRightViewPresenter : UiPresenter<UiTopRightView>
    {
        private readonly IUiSettingsViewPresenter _uiSettingsViewPresenter;

        public UiTopRightViewPresenter(IUiSettingsViewPresenter uiSettingsViewPresenter)
        {
            _uiSettingsViewPresenter = uiSettingsViewPresenter;
        }

        public override void Initialize()
        {
            View.ButtonSettings.OnClickAsObservable()
                .Subscribe(_ => _uiSettingsViewPresenter.Open())
                .AddTo(Disposables);
        }
    }
}