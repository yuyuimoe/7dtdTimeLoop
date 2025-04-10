using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TimeLoop.Enums;

namespace TimeLoop.Models
{
    [Serializable, XmlRoot("TimeLoopConfig")]
    public class ConfigModel
    {
        [XmlElement("Enabled", Type = typeof(bool), IsNullable = false, Order = 0)]
        public bool Enabled { get; set; }
        [XmlElement("Mode", Type = typeof(EMode), IsNullable = false, Order = 1)]
        public EMode Mode { get; set; }
        [XmlArray("Players", IsNullable = false, Order = 2)]
        public List<PlayerModel> Players { get; set; }
        [XmlElement("MinPlayers", Type = typeof(int) ,IsNullable = false, Order = 3)]
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