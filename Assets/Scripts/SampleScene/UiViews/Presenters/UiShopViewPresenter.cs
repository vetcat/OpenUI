using Libs.OpenUI;
using SampleScene.Models.Shop.Settings;
using SampleScene.UiViews.Views.UiItemsShop;
using UnityEngine;

namespace SampleScene.UiViews.Presenters
{
    public class UiShopViewPresenter : UiPresenter<UiShopView>
    {
        private readonly IItemsShopSettings _itemsShopSettings;

        public UiShopViewPresenter(IItemsShopSettings itemsShopSettings)
        {
            _itemsShopSettings = itemsShopSettings;
        }

        public override void Initialize()
        {
            base.Initialize();
            Hide();
            
        }
    }
}