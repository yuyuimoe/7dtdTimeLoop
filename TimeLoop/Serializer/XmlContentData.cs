using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TimeLoop.Enums;
using UnityEngine;

namespace TimeLoop.Serializer
{
    [XmlRoot("TimeLoopSettings")]
    public class XmlContentData
    {

        #region Singleton
        private static XmlContentData? _instance;
        public static XmlContentData Instance
        {
            get { return _instance ??= new XmlContentData(); }
        }
        #endregion

        protected const string FileLocation = "Mods/TimeLoop/TimeLooper.xml";
        protected string AbsoluteFilePath;
        protected DateTime LastModified;

        public bool EnableTimeLooper = true;
        public EMode Mode = EMode.Whitelist;

        [XmlArray("KnownPlayers")]
        public List<Models.PlayerData> PlayerData = new List<Models.PlayerData>();
        public int MinPlayers = 5;

        public static XmlContentData DeserializeInstance()
        {
            string? currentDirectory = Directory.GetParent(Application.dataPath)?.FullName;

            if (currentDirectory == null)
            {
                Log.Exception(new Exception("Could not find the current directory."));
                return Instance;
            }
            
            string absoluteFilePath = Path.Combine(currentDirectory, FileLocation);
            XmlContentData data;

            if (File.Exists(absoluteFilePath))
            {
                //TODO: Add try-catch here.
                Log.Out("[TimeLoop] Loading Config ...");
                data = XmlSerializerWrapper.FromXml<XmlContentData>(absoluteFilePath);
            }
            else
            {
                Log.Out("[TimeLoop] Creating New Config ...");
                data = new XmlContentData();
                XmlSerializerWrapper.ToXml(absoluteFilePath, data);
            }

            data.AbsoluteFilePath = absoluteFilePath;
            data.UpdateLastModified();
            return data;
        }

        protected bool UpdateLastModified()
        {
            DateTime lastModifiedOld = this.LastModified;
            this.LastModified = new FileInfo(this.AbsoluteFilePath).LastWriteTime;
            return lastModifiedOld != this.LastModified;
        }

        public void CheckForUpdate()
        {
            if (UpdateLastModified())
            {
                ReloadConfig();
            }
        }

        public void ReloadConfig()
        {
            if (!File.Exists(this.AbsoluteFilePath)) 
                return;
            
            XmlSerializerWrapper.FromXmlOverwrite(this.AbsoluteFilePath, this);
            Log.Out("[TimeLoop] Config Updated!");
        }

        public void SaveConfig()
        {
            if (!File.Exists(this.AbsoluteFilePath)) 
                return;
            
            XmlSerializerWrapper.ToXml(this.AbsoluteFilePath, this);
            UpdateLastModified();
        }

        public static implicit operator bool(XmlContentData instance)
        {
            return instance != null;
        }
    }
}