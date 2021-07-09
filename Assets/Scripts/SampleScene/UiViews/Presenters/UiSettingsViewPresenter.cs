using Libs.OpenUI;
using Libs.OpenUI.UiEffects;
using SampleScene.UiViews.Views;

namespace SampleScene.UiViews.Presenters
{
    public sealed class UiSettingsViewPresenter : UiPresenter<UiSettingsView>
    {
        public UiSettingsViewPresenter()
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            View.EnableButtonEffects();
            Hide();
            LocalizableInit(View.SettingsLayout);
        }
    }
}
