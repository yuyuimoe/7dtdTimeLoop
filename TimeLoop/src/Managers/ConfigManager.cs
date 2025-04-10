using System;
using System.IO;
using System.Xml;
using TimeLoop.Models;
using TimeLoop.Wrappers;
using UnityEngine;

namespace TimeLoop.Managers
{
    public class ConfigManager
    {
        #region Singleton
        private static ConfigManager? _instance;
        public static ConfigManager Instance{
            get { return _instance ??= new ConfigManager(Main.ConfigFilePath); }
        }
        public static void Instantiate() => _instance = new ConfigManager(Main.ConfigFilePath);
        #endregion
        
        public ConfigModel Config { get; private set; }
        private readonly string _absoluteFilePath;
        private DateTime _lastModified = new DateTime(1970, 1, 1);
        
        private ConfigManager(string fileLocation)
        {
            _absoluteFilePath = GetAbsolutePath(fileLocation);
            Config = LoadConfig();
        }
        
        private bool IsFileModified() => this._lastModified != new FileInfo(this._absoluteFilePath).LastWriteTime;
        
        private string GetAbsolutePath(string relativeFilePath)
        {
            string? gameDirectory = Directory.GetParent(Application.dataPath)?.FullName;
            if (gameDirectory == null)
            {
                Log.Exception(new Exception("Game directory could not be found."));
                return "";
            }
            return Path.Combine(gameDirectory, relativeFilePath);
        }
        
        private ConfigModel LoadConfig()
        {
            ConfigModel configModel = new ConfigModel();
            try
            {
                Log.Out("[TimeLoop] Loading configuration file...");
                configModel = XmlSerializerWrapper.FromXml<ConfigModel>(_absoluteFilePath);
            }
            catch (Exception e) when (e is FileNotFoundException || e is XmlException)
            {
                Log.Error("[TimeLoop] Configuration file is either corrupt or does not exist.");
                Log.Out("[TimeLoop] Creating a configuration file");
                XmlSerializerWrapper.ToXml(_absoluteFilePath, configModel);
            }
            finally
            {
                _lastModified = new FileInfo(_absoluteFilePath).LastWriteTime;
                Log.Out("[TimeLoop] Configuration loaded.");
            }

            return configModel;
        }

        public void UpdateFromFile()
        {
            if (!File.Exists(_absoluteFilePath))
                return;
            
            if (!this.IsFileModified())
                return;
            
            XmlSerializerWrapper.FromXmlOverwrite(this._absoluteFilePath, this.Config);
            Log.Out("[TimeLoop] Configuration updated.");
            TimeLoopManager.Instance.UpdateLoopState();
        }

        public void SaveToFile()
        {
            if (!File.Exists(this._absoluteFilePath))
                return;
            
            XmlSerializerWrapper.ToXml(this._absoluteFilePath, this.Config);
            this._lastModified = new FileInfo(this._absoluteFilePath).LastWriteTime;
        }

        public static implicit operator bool(ConfigManager? instance) => instance != null;
    }
}