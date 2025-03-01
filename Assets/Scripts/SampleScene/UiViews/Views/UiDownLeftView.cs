using EasyButtons;
using Libs.OpenUI;
using Libs.OpenUI.Localization;
using Libs.OpenUI.UiEffects;
using UnityEngine;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views
{
    public class UiDownLeftView : UiView
    {
        public RectTransform Body;
        public Button ButtonItemsShop;
        [Localization("ItemsShop")]
        public Text TextItemsShop;

        [Button]
        private void TestExpandAnimation()
        {
            this.ExpandAnimation(Body, EAnimationTarget.Up);
        }

        [Button]
        private void TestCollapseAnimation()
        {
            this.CollapseAnimation(Body, EAnimationTarget.Down);
        }
    }
}
