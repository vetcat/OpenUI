using System;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;

namespace Libs.OpenUI.Localization
{
    public class LocalizationSetter : ILocalizationSetter, IInitializable, IDisposable
    {
        private readonly List<UiView> _localizedObjects = new();
        private readonly List<UiView> _localizedProjectObjects = new();

        private static ILocalizationProvider _localizationProvider;
        private readonly CompositeDisposable _disposables = new();
        private readonly List<TMP_FontAsset> _defaultFontAssets = new();
        private bool _hasChangeDefaultFontAssets;

        public LocalizationSetter(ILocalizationProvider localizationProvider)
        {
            _localizationProvider = localizationProvider;
        }

        public void Initialize()
        {
            SceneManager.sceneUnloaded += scene => Clear();
            
            _localizationProvider.OnChangeLanguage
                .Subscribe(ChangeLocalization)
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Clear();
        }

        private void ChangeLocalization(string language)
        {
            foreach (var localizedObject in _localizedObjects)
                ApplyLocalization(localizedObject);

            foreach (var localizedObject in _localizedProjectObjects)
                ApplyLocalization(localizedObject);
        }

        public void InitLocalizableProject(UiView localizable)
        {
            if (localizable.LocalizationFields == null || localizable.LocalizationFields.Count == 0)
                return;

            if (!_localizedProjectObjects.Contains(localizable))
            {
                _localizedProjectObjects.Add(localizable);
                ApplyLocalization(localizable);
            }
        }

        public void InitLocalizable(UiView localizable)
        {
            if (localizable.LocalizationFields == null || localizable.LocalizationFields.Count == 0)
                return;

            if (!_localizedObjects.Contains(localizable))
            {
                _localizedObjects.Add(localizable);
                ApplyLocalization(localizable);
            }
        }

        public void Remove(UiView localizable)
        {
            if (_localizedObjects.Contains(localizable))
                _localizedObjects.Remove(localizable);

            if (_localizedProjectObjects.Contains(localizable))
                _localizedObjects.Remove(localizable);
        }

        private void ApplyLocalization(UiView localizedObject)
        {
            foreach (var item in localizedObject.LocalizationFields)
            {
                if (string.IsNullOrEmpty(item.Key))
                    continue;

                if (item.TextField == null)
                    continue;

                item.TextField.text = _localizationProvider.Get(item.Key);
            }
        }

        public void Clear()
        {
            _localizedObjects.Clear();
        }
    }
}