using EasyButtons;
using Libs.OpenUI;
using Libs.OpenUI.UiEffects;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views
{
    public class UiSettingsView : UiViewCanvasGroup
    {
        public Button ButtonClose;
        public Text TextHeader;
        public Toggle ToggleSettings;
        public Toggle ToggleLanguage;
        
        public LanguagesLayout LanguagesLayout;
        public SettingsLayout SettingsLayout;

        [Button]
        private void TestExpandAnimation()
        {
            this.FadeIn(Body, CanvasGroup);
        }

        [Button]
        private void TestCollapseAnimation()
        {
            this.FadeOut(CanvasGroup);
        }
    }
}
