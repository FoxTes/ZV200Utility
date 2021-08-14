using System;
using System.IO;

namespace ZV200Utility.Core.Helpers
{
    /// <summary>
    /// Вспомогательный класс для работы с appsettings.json.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Добавить или обновить значение в appsettings.json.
        /// </summary>
        /// <param name="sectionPathKey">Секция.</param>
        /// <param name="value">Значение.</param>
        /// <typeparam name="T">Тип значения.</typeparam>
        public static void AddOrUpdateAppSetting<T>(string sectionPathKey, T value)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            var json = File.ReadAllText(filePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            SetValueRecursively(sectionPathKey, jsonObj, value);

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, output);
        }

        private static void SetValueRecursively<T>(string sectionPathKey, dynamic jsonObj, T value)
        {
            var remainingSections = sectionPathKey.Split(":", 2);

            var currentSection = remainingSections[0];
            if (remainingSections.Length > 1)
            {
                var nextSection = remainingSections[1];
                SetValueRecursively(nextSection, jsonObj[currentSection], value);
            }
            else
            {
                jsonObj[currentSection] = value;
            }
        }
    }
}