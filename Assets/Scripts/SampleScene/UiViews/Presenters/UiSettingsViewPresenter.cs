using Libs.OpenUI;
using Libs.OpenUI.Localization;
using Libs.OpenUI.UiEffects;
using ProjectContext.Providers;
using SampleScene.UiViews.Views;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SampleScene.UiViews.Presenters
{
    enum EToggleType
    {
        Settings,
        Language
    }

    public sealed class UiSettingsViewPresenter : UiPresenter<UiSettingsView>, IUiSettingsViewPresenter
    {
        private readonly IGameSettingsProvider _gameSettings;
        private readonly ILocalizationProvider _localization;
        private EToggleType _lastToggleFilter;

        public UiSettingsViewPresenter(IGameSettingsProvider gameSettings, ILocalizationProvider localization)
        {
            _gameSettings = gameSettings;
            _localization = localization;
        }

        public override void Initialize()
        {
            base.Initialize();
            View.EnableButtonEffects();
            Hide();
            LocalizableInit(View.SettingsLayout);

            View.ButtonClose.OnClickAsObservable()
                .Subscribe(_ => HideWithAnimation())
                .AddTo(Disposables);

            //filters
            View.ToggleSettings.OnPointerClickAsObservable()
                .Subscribe(_ => { SetFilter(EToggleType.Settings); })
                .AddTo(Disposables);

            View.ToggleLanguage.OnPointerClickAsObservable()
                .Subscribe(_ => { SetFilter(EToggleType.Language); })
                .AddTo(Disposables);

            //set start FilterSetting
            View.ToggleSettings.isOn = true;
            View.SettingsLayout.gameObject.SetActive(true);
            View.LanguagesLayout.gameObject.SetActive(false);
            View.TextHeader.text = Translate("Settings");
            
            //an example of a reactive approach when changing the localization language
            _localization.OnChangeLanguage
                .Subscribe(_ => ChangeLanguage())
                .AddTo(Disposables);

            //inits
            InitSettings();
            InitLanguagesLayout();
        }
        
        public void Open()
        {
            View.transform.SetAsLastSibling();
            Show();
            View.FadeIn(View.Body, View.CanvasGroup);
        }

        private void SetFilter(EToggleType filterType)
        {
            _lastToggleFilter = filterType;
            switch (filterType)
            {
                case EToggleType.Settings:
                    View.SettingsLayout.gameObject.SetActive(true);
                    View.LanguagesLayout.gameObject.SetActive(false);
                    View.TextHeader.text = Translate("Settings");
                    break;

                case EToggleType.Language:
                    View.SettingsLayout.gameObject.SetActive(false);
                    View.LanguagesLayout.gameObject.SetActive(true);
                    View.TextHeader.text = Translate("Language");
                    break;
                default:
                    Debug.LogError("[UiSettingsViewPresenter] SetFilter error not implement logic for " + filterType);
                    break;
            }
        }
        
        private void ChangeLanguage()
        {
            if (View.ToggleSettings.isOn)
                View.TextHeader.text = Translate("Settings");

            if (View.ToggleLanguage.isOn)
                View.TextHeader.text = Translate("Language");
        }

        //example of pattern implementation MVVM (part View -> Model)
        private void InitSettings()
        {
            View.SettingsLayout.SliderMusicVolume.value = _gameSettings.MusicVolume;
            View.SettingsLayout.SliderSoundVolume.value = _gameSettings.SoundVolume;
            View.SettingsLayout.TextSliderMusicValue.text = ((int) (_gameSettings.MusicVolume * 100)).ToString();
            View.SettingsLayout.TextSliderSoundValue.text = ((int) (_gameSettings.SoundVolume * 100)).ToString();

            View.SettingsLayout.SliderMusicVolume.OnValueChangedAsObservable()
                .Subscribe(value =>
                {
                    _gameSettings.MusicVolume = value;
                    View.SettingsLayout.TextSliderMusicValue.text = ((int) (value * 100)).ToString();
                })
                .AddTo(Disposables);

            View.SettingsLayout.SliderSoundVolume.OnValueChangedAsObservable()
                .Subscribe(value =>
                {
                    _gameSettings.SoundVolume = value;
                    View.SettingsLayout.TextSliderSoundValue.text = ((int) (value * 100)).ToString();
                })
                .AddTo(Disposables);
        }
        
        private void InitLanguagesLayout()
        {
            var collectionLanguageItems = View.LanguagesLayout.CollectionLanguageItems;
            collectionLanguageItems.Clear();
            
            foreach (var language in _localization.GetAvailableLanguages)
            {
                var item = collectionLanguageItems.AddItem();
                item.Toggle.group = View.LanguagesLayout.ToggleGroup;
                item.TextLanguage.text = language.ToString();
                item.Language = language;
                item.Toggle.isOn = false;
            
                item.Toggle.OnPointerClickAsObservable()
                    .Subscribe(_ => { LanguageChanged(language); })
                    .AddTo(item);
            }
            
            var currentLanguage = _localization.GetCurrentLanguage();
            var currentLanguageItem = collectionLanguageItems.GetItems().Find(f => f.Language == currentLanguage);
            currentLanguageItem.Toggle.isOn = true;
        }
        
        private void LanguageChanged(SystemLanguage selectedLanguage)
        {
            var currentLanguage = _localization.GetCurrentLanguage();

            if (currentLanguage != selectedLanguage)
                _localization.ChangeLanguage(selectedLanguage);

            SetFilter(_lastToggleFilter);
        }
    }
}