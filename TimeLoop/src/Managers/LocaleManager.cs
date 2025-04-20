﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static SimpleJson2.SimpleJson2;

namespace TimeLoop.Managers {
    public class LocaleManager {
        private bool _isFallbackMode;
        private Dictionary<string, string> _localeDict;
        public List<string> LocaleList;

        private LocaleManager(string locale) {
            _localeDict = LoadLocale(locale);
            LocaleList = GetLocales();
        }

        public string LoadedLocale { get; private set; } = null!;

        private string GetLocalePath(string locale) {
            return Main.GetAbsolutePath(Path.Combine(Main.LocaleFolderPath, locale + ".json"));
        }

        public void SetLocale(string newLocale) {
            _localeDict = LoadLocale(newLocale);
        }

        private List<string> GetLocales() {
            try {
                var localePath = Main.GetAbsolutePath(Main.LocaleFolderPath);
                var locales = Directory.GetFiles(localePath, "*.json", SearchOption.TopDirectoryOnly);
                return locales.Select(Path.GetFileNameWithoutExtension).ToList();
            }
            catch (Exception e) {
                Log.Error("[TimeLoop] Failed to get all locale files. {0}", e.Message);
#if DEBUG
                Log.Exception(e);
#endif
                return new List<string>();
            }
        }

        private Dictionary<string, string> LoadLocale(string locale) {
            try {
                var localePath = TryGetValidLocale(locale) ?? TryGetValidLocale("en_us");
                if (localePath == null) {
                    Log.Error(
                        "[TimeLoop] Failed to load any locale file. Using raw keys. (Use tl_locale or review the config file)");
                    _isFallbackMode = true;
                    LoadedLocale = "invalid_locale";
                    return new Dictionary<string, string>();
                }

                LoadedLocale = Path.GetFileNameWithoutExtension(localePath);
                using var stream = new StreamReader(localePath);
                return DeserializeObject<Dictionary<string, string>>(stream.ReadToEnd());
            }
            catch (Exception e) {
                Log.Error("[TimeLoop] Failed to load localization file. {0}", e.Message);
#if DEBUG
                Log.Exception(e);
#endif
                _isFallbackMode = true;
                return new Dictionary<string, string>();
            }
        }

        private string? TryGetValidLocale(string locale) {
            var path = GetLocalePath(locale);
            if (File.Exists(path)) return path;
            Log.Error("[TimeLoop] Failed to load locale file for '{0}'", locale);
            return null;
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
            get { return _instance ??= new LocaleManager(ConfigManager.Instance.Config.Language); }
        }

        public static void Instantiate(string locale) {
            _instance = new LocaleManager(locale);
        }

        #endregion
    }
}