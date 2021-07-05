using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Libs.OpenUI.Editor
{
    public static class DataHelper
    {
        public static void WriteJson<T>(string path, T data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(path, json);
            AssetDatabase.Refresh();
        }
        
        public static T LoadJson<T>(string path, string name)
        {
            var fileName = $"{name}.json";
            var filePath = Path.Combine(path, fileName);

            if (File.Exists(filePath))
            {
                var dataAsJson = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(dataAsJson);
            }
            else
            {
                Debug.LogError($"Missing {fileName}!");
                return default;
            }
        }
    }
}