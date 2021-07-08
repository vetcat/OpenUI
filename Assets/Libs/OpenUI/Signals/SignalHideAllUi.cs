namespace Libs.OpenUI.Signals
{
    public readonly struct SignalHideAllUi
    {
        public readonly IUiPresenter UiPresenter;

        public SignalHideAllUi(IUiPresenter uiPresenter)
        {
            UiPresenter = uiPresenter;
        }
    }
}