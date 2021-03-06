using EasyButtons;
using Libs.OpenUI;
using Libs.OpenUI.UiEffects;
using UnityEngine;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views
{
    public class UiTopRightView : UiView
    {
        public RectTransform Body;
        public Image ImageIconCoins;
        public Text TextCoinsAmount;
     
        [Button]
        private void TestExpandAnimation()
        {
            this.ExpandAnimation(Body, EAnimationTarget.Down);
        }

        [Button]
        private void TestCollapseAnimation()
        {
            this.CollapseAnimation(Body, EAnimationTarget.Up);
        }
    }
}
