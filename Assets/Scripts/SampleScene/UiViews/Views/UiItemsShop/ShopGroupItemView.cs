using Libs.OpenUI;
using Libs.OpenUI.Localization;
using SampleScene.Models.Shop;
using UnityEngine;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views.UiItemsShop
{
    public class ShopGroupItemView : UiView, ILocalizable
    {
        public Toggle Toggle;
        public Image ImageIcon;
        public Image ImageGlow;
        [HideInInspector] public EItemShopGroup Group;
    }
}
