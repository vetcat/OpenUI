namespace Libs.OpenUI
{
    public interface IWindowsController
    {
        void HideAll(IUiController excludeUiController);
        void RestoreHidden();
    }
}