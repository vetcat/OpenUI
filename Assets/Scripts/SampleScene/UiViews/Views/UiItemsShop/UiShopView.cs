using EasyButtons;
using Libs.OpenUI;
using Libs.OpenUI.Localization;
using Libs.OpenUI.UiEffects;
using UnityEngine;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views.UiItemsShop
{
    public class UiShopView : UiView, ILocalizable
    {
        public RectTransform Body;
        public Button ButtonClose;
        public CollectionShopGroupItem CollectionShopGroupItem; 
        public CollectionShopItem CollectionShopItem; 

        [Button]
        private void TestExpandAnimation()
        {
            this.ExpandAnimation(Body, EAnimationTarget.Right);
        }

        [Button]
        private void TestCollapseAnimation()
        {
            this.CollapseAnimation(Body, EAnimationTarget.Left);
        }
    }
}
