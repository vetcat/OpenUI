using Libs.OpenUI;
using UnityEngine;
using UnityEngine.UI;

namespace SampleScene.UiViews.Views
{
    public class LanguageItem : UiView
    {
        public Toggle Toggle;
        public Text TextLanguage;
        [HideInInspector] public SystemLanguage Language;
    }
}
