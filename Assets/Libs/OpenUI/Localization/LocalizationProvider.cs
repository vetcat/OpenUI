using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;
using Zenject;

namespace Libs.OpenUI.Localization
{
    public class LocalizationProvider : ILocalizationProvider, IInitializable
    {
        public List<SystemLanguage> GetAvailableLanguages { get; private set; }
        public IObservable<SystemLanguage> OnChangeLanguage => _onChangeLanguage;
        private readonly Subject<SystemLanguage> _onChangeLanguage;

        private Dictionary<string, string> _languageSet = new Dictionary<string, string>();

        private SystemLanguage _selectedLanguage;

        private const string LanguageKey = "Language";

        private const SystemLanguage DefaultLanguage = SystemLanguage.English;

        public LocalizationProvider()
        {
            _onChangeLanguage = new Subject<SystemLanguage>();
        }

        public void Initialize()
        {
            InitAvailableLanguages();

            if (PlayerPrefs.HasKey(LanguageKey))
            {
                _selectedLanguage = GetAvailableLanguages.Contains((SystemLanguage)PlayerPrefs.GetInt(LanguageKey))
                    ? (SystemLanguage)PlayerPrefs.GetInt(LanguageKey)
                    : DefaultLanguage;
            }
            else
            {
                _selectedLanguage = GetAvailableLanguages.Contains(Application.systemLanguage)
                    ? Application.systemLanguage
                    : DefaultLanguage;
            }

            ChangeLanguage(_selectedLanguage);
        }

        private void InitAvailableLanguages()
        {
            GetAvailableLanguages = new List<SystemLanguage>();

            
            var languagesAsset = Resources.Load<TextAsset>("Languages/Info");            

            if (languagesAsset == null)
            {
                Debug.LogError("Error : LocalizationProvider InitAvailableLanguages not found Info.json");
                return;
            }

            var languages = JsonConvert.DeserializeObject<List<string>>(languagesAsset.text);

            foreach (var language in languages)
                if (Enum.TryParse<SystemLanguage>(language, out var systemLanguage))
                    GetAvailableLanguages.Add(systemLanguage);
        }

        public string Get(string key, params object[] args)
        {
            if (!_languageSet.ContainsKey(key))
            {
                Debug.LogError("LocalizationProvider key not found " + key);
                return key;
            }

            if (args == null)
                return _languageSet[key];

            string result;
            try
            {
                result = string.Format(_languageSet[key], args).Replace(@"\n", "\n");
            }
            catch (Exception e)
            {
                result = "ERROR: " + e.Message;
            }


            return result;
        }

        public bool HasKey(string key)
        {
            return _languageSet.ContainsKey(key);
        }

        public SystemLanguage GetCurrentLanguage()
        {
            return _selectedLanguage;
        }

        public void ChangeLanguage(SystemLanguage language)
        {
            if (!GetAvailableLanguages.Contains(language))
            {
                Debug.LogError("LocalizationProvider ChangeLanguage Localization not found for language = " + language);
                return;
            }

            _selectedLanguage = language;
            SetLanguage(_selectedLanguage);
            SaveSelectedLanguage(_selectedLanguage);
            _onChangeLanguage.OnNext(language);
        }

        public string GetTime(TimeSpan time)
        {
            var res = string.Empty;

            if (time.Days > 0)
                res += $"{time.Days}{Get("day")}";

            if (time.Hours > 0)
                res += $" {time.Hours}{Get("hour")}";

            if (time.Minutes > 0 & time.Days == 0)
                res += $" {time.Minutes}{Get("min")}";

            if (time.Seconds > 0 & time.Days == 0 & time.Hours == 0)
                res += $" {time.Seconds}{Get("seconds")}"; 

            return res;
        }

        private void SetLanguage(SystemLanguage language)
        {
            var languagesAsset = Resources.Load<TextAsset>($"Languages/{language}");
            if (languagesAsset == null)
            {
                Debug.LogError("LocalizationProvider SetLanguage Localization not found for language = " + language);
                return;
            }

            _languageSet = JsonConvert.DeserializeObject<Dictionary<string, string>>(languagesAsset.text);
        }
        
        private void SaveSelectedLanguage(SystemLanguage language)
        {
            PlayerPrefs.SetInt(LanguageKey, (int) language);
            PlayerPrefs.Save();
        }

        public void ChangeLocalization()
        {
            var current = GetCurrentLanguage();
            var index = GetAvailableLanguages.IndexOf(current);

            ChangeLanguage(index > 0
                ? GetAvailableLanguages[0]
                : GetAvailableLanguages[1]);
        }

        public int GetTemplateCount(string template)
        {
            var count = 0;
            foreach (var key in _languageSet.Keys)
            {
                if (key.Contains(template))
                    count++;
            }

            return count;
        }
    }
}