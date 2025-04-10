using System;
using System.Xml.Serialization;
using UnityEngine.Serialization;

namespace TimeLoop.Models
{
    [Serializable, XmlRoot("Player")]
    public class PlayerModel
    {

        [XmlAttribute("ID", Type = typeof(string))]
        public string id;
        
        [XmlAttribute("Name", typeof(string))]
        public string playerName;
        
        [XmlAttribute("Whitelisted", typeof(bool))]
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
