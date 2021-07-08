namespace Libs.OpenUI
{
    public interface IWindowsController
    {
        void HideAll(IUiPresenter excludeUiPresenter);
        void RestoreHidden();
    }
}