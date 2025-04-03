using System.Xml.Serialization;

namespace TimeLoop.Enums
{
    public enum EMode
    {
        [XmlEnum(Name = "none")]
        Disabled,
        [XmlEnum(Name = "whitelist")]
        Whitelist,
        [XmlEnum(Name = "threshold")]
        MinPlayerCount,
        [XmlEnum(Name = "whitelisted_threshold")]
        MinWhitelistPlayerCount
    }
}