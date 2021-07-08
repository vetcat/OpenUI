using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Libs.OpenUI.Editor
{
    public class LocalizationEditorWindow : EditorWindow
    {
        private string _fileContent;
        private static readonly List<string> _writeLanguages = new List<string>();
        private int _keysCount;

        [MenuItem("Tools/Localization")]
        private static void ShowWindow()
        {
            var window = GetWindow<LocalizationEditorWindow>();
            window.titleContent = new GUIContent("Localization");
            window.Show();
            _writeLanguages.Clear();
        }

        private void OnGUI()
        {
            GUILayout.Space(15);
            if (GUILayout.Button("Select .scv file...", GUILayout.Height(40), GUILayout.Width(140)))
            {
                SelectFile();
            }

            GUILayout.Space(15);
            if (!string.IsNullOrEmpty(_fileContent))
            {
                if (GUILayout.Button("Write", GUILayout.Height(40), GUILayout.Width(140)))
                {
                    LoadScv(_fileContent);
                }
            }

            GUILayout.Space(15);
            if (_writeLanguages.Count > 0)
            {
                foreach (var language in _writeLanguages)
                {
                    EditorGUILayout.LabelField("language created: ",
                        language);
                }

                EditorGUILayout.LabelField("keys count: ", _keysCount.ToString());

                Repaint();
            }
        }

        private void SelectFile()
        {
            string path = EditorUtility.OpenFilePanel("Select source scv file", "", "csv");
            if (path.Length != 0)
            {
                _fileContent = File.ReadAllText(path);
            }

            if (string.IsNullOrEmpty(_fileContent))
            {
                Debug.LogError($"error : selected file is empty, path = {path}");
            }
        }

        private void LoadScv(string scvData)
        {
            var data = CsvReader.CsvToDict(scvData, out var duplicatedKeys);
            if (data.Count == 0)
            {
                Debug.LogError("error no data");
                return;
            }

            var header = data.ElementAt(0);
            _keysCount = data.Keys.Count;
            var languageSets = new List<Dictionary<string, string>>(header.Value.Length);
            languageSets.AddRange(header.Value.Select(language => new Dictionary<string, string>()));

            foreach (var d in data)
            {
                if (d.Equals(header))
                    continue;

                var index = 0;
                foreach (var v in d.Value)
                {
                    languageSets[index].Add(d.Key, v);
                    index++;
                }
            }

            var indexLanguage = 0;
            _writeLanguages.Clear();
            foreach (var language in header.Value)
            {
                if (string.IsNullOrEmpty(language))
                    continue;

                WriteLanguageSet(language, languageSets[indexLanguage]);
                _writeLanguages.Add(language);
                indexLanguage++;
            }

            WriteLanguagesInfo(_writeLanguages);

            _fileContent = null;

            if (duplicatedKeys.Count > 0)
            {
                ShowError(duplicatedKeys);
            }
        }

        private void WriteLanguageSet(string language, Dictionary<string, string> languageSet)
        {
            var fileName = $"{language}.json";
            var assetsPath = Path.Combine(Application.dataPath, "Resources/Languages");
            var filePath = Path.Combine(assetsPath, fileName);

            DataHelper.WriteJson(filePath, languageSet);
            AssetDatabase.Refresh();
        }

        private void WriteLanguagesInfo(List<string> languages)
        {
            var fileName = "Info.json";
            var assetsPath = Path.Combine(Application.dataPath, "Resources/Languages");
            var filePath = Path.Combine(assetsPath, fileName);

            DataHelper.WriteJson(filePath, languages);
            AssetDatabase.Refresh();
        }

        private void ShowError(List<string> duplicatedKeys)
        {
            var dupList = duplicatedKeys.Aggregate(string.Empty, (current, key) => current + $"{key}\n");
            EditorGUILayout.HelpBox($"duplicated key {dupList}", MessageType.Error);
            ShowNotification(new GUIContent($"has duplicated key:\n{dupList}"));
            Debug.LogError($"has duplicated keys: \n{dupList}");
        }
    }
}