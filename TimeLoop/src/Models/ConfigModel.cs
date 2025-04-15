using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TimeLoop.Enums;

namespace TimeLoop.Models
{
    [Serializable, XmlRoot("TimeLoopConfig")]
    public class ConfigModel
    {
        [XmlElement("Enabled")]
        public bool Enabled { get; set; }
        [XmlElement("Mode")]
        public EMode Mode { get; set; }
        [XmlArray("Players")]
        public List<PlayerModel> Players { get; set; }
        [XmlElement("MinPlayers")]
        public int MinPlayers { get; set; }
        [XmlElement("DaysToSkip")]
        public int DaysToSkip { get; set; }
        [XmlElement("LoopLimit")]
        public int LoopLimit { get; set; }

        public ConfigModel()
        {
            this.Enabled = true;
            this.Mode = EMode.Whitelist;
            this.Players = new List<PlayerModel>();
            this.MinPlayers = 5;
            this.DaysToSkip = 0;
            this.LoopLimit = 0;
        }

        public ConfigModel(bool enabled, EMode mode, List<PlayerModel> players, int minPlayers, int daysToSkip, int loopLimit)
        {
            this.Enabled = enabled;
            this.Mode = mode;
            this.Players = players;
            this.MinPlayers = minPlayers;
            this.DaysToSkip = daysToSkip;
            this.LoopLimit = loopLimit;
        }
    }
}