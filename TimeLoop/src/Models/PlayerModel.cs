﻿using System;
using System.Xml.Serialization;
using UnityEngine.Serialization;

namespace TimeLoop.Models
{
    [Serializable]
    public class PlayerModel
    {

        [FormerlySerializedAs("ID")] [XmlAttribute]
        public string id;
        
        [FormerlySerializedAs("PlayerName")] [XmlAttribute]
        public string playerName;
        
        [FormerlySerializedAs("SkipTimeLoop")] [XmlAttribute]
        public bool skipTimeLoop;

        public PlayerModel()
        {
            this.id = Guid.NewGuid().ToString();
            this.playerName = string.Empty;
            this.skipTimeLoop = false;
        }
        
        public PlayerModel(ClientInfo clientInfo)
        {
            this.id = clientInfo.PlatformId.CombinedString;
            this.playerName = clientInfo.playerName;
            this.skipTimeLoop = false;
        }
    }
}
