using System;
using System.Collections.Generic;
using System.Reflection;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Libs.OpenUI.Localization
{
    public class LocalizationSetter : ILocalizationSetter, IInitializable, IDisposable
    {
        private readonly List<LocalizedObject> _localizedObjects = new List<LocalizedObject>();

        private static ILocalizationProvider _localizationProvider;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

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

        private void ChangeLocalization(SystemLanguage language)
        {
            foreach (var localizedObject in _localizedObjects)
                ApplyLocalization(localizedObject);
        }

        public void InitLocalizable(ILocalizable localizable)
        {
            var fields = localizable.GetType().GetFields();
            foreach (var fieldInfo in fields)
            {
                var localAttrDefined = Attribute.IsDefined(fieldInfo, typeof(LocalizationAttribute));
                if (!localAttrDefined) continue;

                if (fieldInfo.FieldType != typeof(Text))
                    throw new ArgumentException("Localization attribute can be applied only to UI.Text, localizable: " +
                                                localizable.GetType().Name);

                var label = fieldInfo.GetValue(localizable) as Text;
                if (label == null)
                    throw new ArgumentException(
                        "Localization attribute can be applied only to UI.Text and not null, localizable: " +
                        localizable.GetType().Name);

                var attr = fieldInfo.GetCustomAttribute<LocalizationAttribute>();
                var localizedObject = new LocalizedObject(label, attr);

                ApplyLocalization(localizedObject);
                _localizedObjects.Add(localizedObject);
            }
        }

        public void Remove(ILocalizable localizable)
        {
            var fields = localizable.GetType().GetFields();
            foreach (var fieldInfo in fields)
            {
                var localAttrDefined = Attribute.IsDefined(fieldInfo, typeof(LocalizationAttribute));
                if (!localAttrDefined) continue;

                if (fieldInfo.FieldType != typeof(Text))
                    throw new ArgumentException("Localization attribute can be applied only to UI.Text, localizable: " +
                                                localizable.GetType().Name);

                var label = fieldInfo.GetValue(localizable) as Text;
                if (label == null)
                    throw new ArgumentException(
                        "Localization attribute can be applied only to UI.Text and not null, localizable: " +
                        localizable.GetType().Name);

                Remove(label);
            }
        }

        private void Remove(Text label)
        {
            var localizedObject = _localizedObjects.Find(f => f.Text == label);
            _localizedObjects.Remove(localizedObject);
        }

        private void ApplyLocalization(LocalizedObject localizedObject)
        {
            if (localizedObject.Text == null)
            {
                Debug.LogError("LocalizationSetter ApplyLocalization error localizedObject is null " +
                               localizedObject.Attribute.Value);
                _localizedObjects.Remove(localizedObject);
                return;
            }

            var localizedText = _localizationProvider.Get(localizedObject.Attribute.Value, localizedObject.Args);
            localizedObject.Text.text = localizedText;
        }

        public string CreateLocalizationPattern(Text text, string key, params object[] args)
        {
            var localizedObject = _localizedObjects.Find(f => f.Text == text);
            localizedObject.SetArgs(args);
            return _localizationProvider.Get(key, args);
        }

        public void Clear()
        {
            _localizedObjects.Clear();
        }

        private class LocalizedObject
        {
            public readonly Text Text;
            public readonly LocalizationAttribute Attribute;
            public object[] Args;

            public LocalizedObject(Text text, LocalizationAttribute attribute)
            {
                Text = text;
                Attribute = attribute;
            }

            public void SetArgs(object[] args)
            {
                Args = args;
            }
        }
    }
}