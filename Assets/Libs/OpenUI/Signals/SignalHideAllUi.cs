namespace Libs.OpenUI.Signals
{
    public readonly struct SignalHideAllUi
    {
        public readonly IUiController UiController;

        public SignalHideAllUi(IUiController uiController)
        {
            UiController = uiController;
        }
    }
}