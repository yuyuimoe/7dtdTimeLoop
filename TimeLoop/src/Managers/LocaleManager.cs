using System;
using System.Collections.Generic;
using System.IO;
using static SimpleJson2.SimpleJson2;

namespace TimeLoop.Managers {
    public class LocaleManager {
        private bool _isFallbackMode;
        private Dictionary<string, string> _localeDict = null!;

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
                _localeDict = DeserializeObject<Dictionary<string, string>>(stream.ReadToEnd());
                stream.Close();
            }
            catch (Exception e) {
                Log.Error("[TimeLoop] Failed to load localization file. {0}", e.Message);
#if DEBUG
                Log.Exception(e);
#endif
                _isFallbackMode = true;
                _localeDict = new Dictionary<string, string>();
            }
        }

        public string Localize(string key) {
            if (_isFallbackMode || !_localeDict.TryGetValue(key, out var locale)) return key;

            return locale;
        }

        public string Localize(string key, params object[] args) {
            return string.Format(Localize(key), args);
        }

        public string LocalizeWithPrefix(string key) {
            return string.Join(Localize("prefix"), Localize(key));
        }

        public string LocalizeWithPrefix(string key, params object[] args) {
            return string.Join(Localize("prefix"), Localize(key, args));
        }

        #region Singleton

        private static LocaleManager? _instance;

        public static LocaleManager Instance {
            get { return _instance ??= new LocaleManager(); }
        }

        public static void Instantiate(string locale) {
            _instance = new LocaleManager(locale);
        }

        #endregion
    }
}