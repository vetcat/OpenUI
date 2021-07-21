namespace Libs.OpenUI.Signals
{
    public readonly struct SignalHideView
    {
        public readonly UiView View;
        public SignalHideView(UiView view)
        {
            View = view;
        }
    }
}