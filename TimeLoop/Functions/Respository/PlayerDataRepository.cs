using Platform.Local;
using Platform.Steam;
using ContentData = TimeLoop.Functions.Serializer.XmlContentData;

namespace TimeLoop.Functions.Player
{
    public class PlayerDataRepository
    {
        private readonly ClientInfo _clientInfo;
        private readonly ContentData _contentData;

        public PlayerDataRepository(ClientInfo clientInfo, ContentData contentData)
        {
            _clientInfo = clientInfo;
            _contentData = contentData;
        }

        public Serializer.PlayerData? GetPlayerData()
        {
            Log.Out($"Called GetPlayerData of type {_clientInfo.PlatformId.GetType()}");
            return _contentData.PlayerData?.Find(data =>
            {
                if (data == null)
                    return false;

                if (data.ID == _clientInfo.CrossplatformId?.CombinedString)
                    return true;
                
                return _clientInfo.PlatformId switch
                {
                    UserIdentifierSteam steamUser => steamUser.SteamId.ToString() == data.ID,
                    UserIdentifierLocal localUser => localUser.CombinedString == data.ID,
                    _ => false
                };
            });
        }
    }
}