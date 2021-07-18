using Libs.OpenUI;
using Libs.OpenUI.Localization;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views.UiItemsShop
{
    public class ShopItemView : UiView, ILocalizable
    {
        public Button ButtonItem;
        public Image ImageIcon;
        public Text TextName;
        public Text TextAmount;
    }
}
