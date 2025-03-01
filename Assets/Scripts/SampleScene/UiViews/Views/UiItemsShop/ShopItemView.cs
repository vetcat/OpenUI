using Libs.OpenUI;
using SampleScene.Models.Shop;
using UnityEngine;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views.UiItemsShop
{
    public class ShopItemView : UiView
    {
        public Button ButtonItem;
        public Image ImageIcon;
        public Text TextName;
        public Text TextAmount;
        [HideInInspector] public EItemShopType Type;
    }
}
