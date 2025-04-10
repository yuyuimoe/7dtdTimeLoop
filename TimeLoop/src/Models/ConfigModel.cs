using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TimeLoop.Enums;

namespace TimeLoop.Models
{
    [Serializable, XmlRoot("TimeLoopConfig")]
    public class ConfigModel
    {
        [XmlAttribute("enabled")]
        public bool Enabled { get; set; }
        [XmlAttribute("mode")]
        public EMode Mode { get; set; }
        [XmlArray("Players")]
        public List<PlayerModel> Players { get; set; }
        [XmlAttribute("minPlayers")]
        public int MinPlayers { get; set; }

        public ConfigModel()
        {
            this.Enabled = true;
            this.Mode = EMode.Whitelist;
            this.Players = new List<PlayerModel>();
            this.MinPlayers = 5;
        }

        public ConfigModel(bool enabled, EMode mode, List<PlayerModel> players, int minPlayers)
        {
            this.Enabled = enabled;
            this.Mode = mode;
            this.Players = players;
            this.MinPlayers = minPlayers;
        }
    }
}