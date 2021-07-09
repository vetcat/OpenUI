using Libs.OpenUI;
using Libs.OpenUI.Localization;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views
{
    public class SettingsLayout : UiView, ILocalizable
    {
        public Slider SliderMusicVolume;
        public Slider SliderSoundVolume;
        [Localization("TextMusic")]
        public Text TextMusic;
        [Localization("TextSound")]
        public Text TextSound;
        public Text TextSliderMusicValue;
        public Text TextSliderSoundValue;
    }
}
