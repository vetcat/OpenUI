using System;
using System.Collections.Generic;
using UnityEngine;

namespace Libs.OpenUI.Localization
{
    public interface ILocalizationProvider
    {
        IObservable<SystemLanguage> OnChangeLanguage { get; }
        string Get(string key, params object[] args);
        bool HasKey(string key);
        SystemLanguage GetCurrentLanguage();
        List<SystemLanguage> GetAvailableLanguages { get; }
        void ChangeLanguage(SystemLanguage language);
        string GetTime(TimeSpan time);
        void ChangeLocalization();
        int GetTemplateCount(string template);
    }
}