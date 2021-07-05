/*
 * part code from open source project GoogleDocs Data Downloader (MIT license)
 * https://github.com/Leopotam/googledocs-import
 */

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Libs.OpenUI
{
    public class CsvReader
    {
        static readonly List<string> _csvBuffer = new List<string>(32);
        static readonly Regex CsvParseRegex = new Regex("(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)");

        static void ParseCsvLine(string data)
        {
            _csvBuffer.Clear();
            foreach (Match m in CsvParseRegex.Matches(data))
            {
                var part = m.Value.Trim();
                if (part.Length > 0)
                {
                    if (part[0] == '"' && part[part.Length - 1] == '"')
                    {
                        part = part.Substring(1, part.Length - 2);
                    }

                    part = part.Replace("\"\"", "\"");
                }

                _csvBuffer.Add(part);
            }
        }

        public static Dictionary<string, string[]> CsvToDict(string data, out List<string> duplicatedKeys)
        {
            var list = new Dictionary<string, string[]>();
            var headerLen = -1;
            string key;
            duplicatedKeys = new List<string>();
            using (var reader = new StringReader(data))
            {
                while (reader.Peek() != -1)
                {
                    var line = reader.ReadLine();
                    //Debug.LogError("reader.ReadLine() = " + line);
                    ParseCsvLine(line);
                    if (_csvBuffer.Count > 0 && !string.IsNullOrEmpty(_csvBuffer[0]))
                    {
                        if (headerLen == -1)
                        {
                            headerLen = _csvBuffer.Count;
                        }

                        if (_csvBuffer.Count != headerLen)
                        {
                            Debug.LogError("Invalid csv line, skipping.");
                            continue;
                        }

                        key = _csvBuffer[0];
                        _csvBuffer.RemoveAt(0);

                        if (list.ContainsKey(key))
                        {
                            if (!duplicatedKeys.Contains(key))
                                duplicatedKeys.Add(key);
                        }
                        else
                        {
                            list[key] = _csvBuffer.ToArray();
                        }
                    }
                }
            }

            return list;
        }
    }
}