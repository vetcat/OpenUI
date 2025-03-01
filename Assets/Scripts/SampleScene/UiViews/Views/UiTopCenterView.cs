using EasyButtons;
using Libs.OpenUI;
using Libs.OpenUI.Localization;
using Libs.OpenUI.UiEffects;
using UnityEngine;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views
{
    public class UiTopCenterView : UiView
    {
        public RectTransform Body;
        public Image HintArea;
        public Text TextLocalTime;
        [Localization("PressAndHold")]
        public Text TextPressAndHold;

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
