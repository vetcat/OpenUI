using EasyButtons;
using Libs.OpenUI;
using Libs.OpenUI.Localization;
using Libs.OpenUI.UiEffects;
using UnityEngine;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views
{
    public class UiDownRightView : UiView
    {
        public RectTransform Body; 
        public Button ButtonSettings;
        [Localization("Settings")]
        public Text TextSettings;
        
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
