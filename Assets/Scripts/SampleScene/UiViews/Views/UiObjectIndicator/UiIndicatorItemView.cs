using Libs.OpenUI;
using Libs.OpenUI.Localization;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views.UiObjectIndicator
{
    public class UiIndicatorItemView : UiView
    {
        public Image ImageIcon;
        public Button ButtonAction;
        public Text TextInfo;
        public Slider Slider;
        public Text TextSliderValue;
        [Localization("ClickToReward")]
        public Text TextClickToReward;
    }
}