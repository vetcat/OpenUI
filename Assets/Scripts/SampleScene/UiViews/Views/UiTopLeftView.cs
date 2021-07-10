using EasyButtons;
using Libs.OpenUI;
using Libs.OpenUI.Localization;
using Libs.OpenUI.UiEffects;
using UnityEngine;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views
{
    public class UiTopLeftView : UiView, ILocalizable
    {
        public RectTransform Body;
        public Text TextPlayerName;

        public PlayerDataLayout HealthData;
        public PlayerDataLayout XpData;

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
