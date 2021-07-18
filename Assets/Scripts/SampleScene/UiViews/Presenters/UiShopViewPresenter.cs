using Libs.OpenUI;
using SampleScene.UiViews.Views.UiItemsShop;

namespace SampleScene.UiViews.Presenters
{
    public class UiShopViewPresenter : UiPresenter<UiShopView>
    {
        public UiShopViewPresenter()
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            Hide();
        }
    }
}