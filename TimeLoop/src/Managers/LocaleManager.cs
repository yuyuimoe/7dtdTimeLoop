using System;
using System.Collections.Generic;
using System.IO;
using static SimpleJson2.SimpleJson2;

namespace TimeLoop.Managers {
    public class LocaleManager {
        private bool _isFallbackMode = false;
        private List<string> _localeJson;

        private LocaleManager(string locale = "en_us") {
            LoadLocale(locale);
        }

        public void SetLocale(string newLocale) {
            LoadLocale(newLocale);
        }

        private void LoadLocale(string locale = "en_us") {
            try {
                var localePath = Main.GetAbsolutePath(Path.Combine(Main.LocaleFolderPath, locale + ".json"));
                using var stream = new StreamReader(localePath);
                _localeJson = DeserializeObject<List<string>>(stream.ReadToEnd());
            }
            catch (Exception e) {
                Log.Error("[TimeLoop] Failed to load localization file.");
                _isFallbackMode = true;
            }
        }

        public string Translate(string key, params object[] args) {
            if (_isFallbackMode || !_localeJson.Contains(key)) return string.Join(" ", key, args);
            return string.Format(_localeJson.Find(s => s.Equals(key)), args);
        }

        #region Singleton

        private static LocaleManager _instance;

        public static LocaleManager Instance {
            get { return _instance ??= new LocaleManager(); }
        }

        public void Instantiate(string locale) {
            _instance = new LocaleManager(locale);
        }

        #endregion
    }
}