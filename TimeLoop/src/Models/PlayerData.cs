using System;
using System.Xml.Serialization;
using UnityEngine.Serialization;

namespace TimeLoop.Models
{
    [Serializable]
    public class PlayerData
    {

        [FormerlySerializedAs("ID")] [XmlAttribute]
        public string id;
        
        [FormerlySerializedAs("PlayerName")] [XmlAttribute]
        public string playerName;
        
        [FormerlySerializedAs("SkipTimeLoop")] [XmlAttribute]
        public bool skipTimeLoop;

        public PlayerData()
        {
            this.id = Guid.NewGuid().ToString();
            this.playerName = string.Empty;
            this.skipTimeLoop = false;
        }
        
        public PlayerData(ClientInfo clientInfo)
        {
            this.id = clientInfo.PlatformId.CombinedString;
            this.playerName = clientInfo.playerName;
            this.skipTimeLoop = false;
        }
    }
}
