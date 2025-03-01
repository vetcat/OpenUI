using EasyButtons;
using Libs.OpenUI;
using Libs.OpenUI.Localization;
using Libs.OpenUI.UiEffects;
using UnityEngine;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views
{
    public class UiTopLeftView : UiView
    {
        public RectTransform Body;
        public Text TextPlayerName;
        [Localization("Level")]
        public Text TextLevel;
        public Text TextLevelValue;

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
