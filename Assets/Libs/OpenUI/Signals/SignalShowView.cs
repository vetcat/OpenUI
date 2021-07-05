namespace Libs.OpenUI.Signals
{
    public readonly struct SignalShowView
    {
        public readonly UiView View;
        public SignalShowView(UiView view)
        {
            View = view;
        }
    }
}